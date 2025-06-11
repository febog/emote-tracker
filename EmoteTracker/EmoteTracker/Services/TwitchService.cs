using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmoteTracker.Services
{
    public class TwitchService : ITwitchService
    {
        private readonly HttpClient _httpClient;
        private readonly TwitchServiceOptions _options;
        private readonly IMemoryCache _memoryCache;

        private const string AppAccessTokenCacheKey = "twitch_app_access_token";

        public TwitchService(HttpClient httpClient, IOptions<TwitchServiceOptions> options, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _memoryCache = memoryCache;
        }

        public async Task<string> GetTwitchId(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;

            // Twitch limits usernames to 25 characters
            if (username.Length > 25) return null;

            string query = Uri.EscapeDataString(username.Trim());

            string apiUrl = $"https://api.twitch.tv/helix/users?login={query}";
            if (!Uri.TryCreate(apiUrl, UriKind.Absolute, out Uri uri)) return null;

            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = uri;
                request.Headers.Add("Client-Id", _options.ClientId);
                request.Headers.Add("Authorization", $"Bearer {await GetAppAccessToken()}");
                var response = await _httpClient.SendAsync(request);

                // If token fails, retry once with a forced fresh token
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    request.Headers.Add("Authorization", $"Bearer {await GetAppAccessToken(true)}");
                    response = await _httpClient.SendAsync(request);
                }

                var content = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var data = await JsonSerializer.DeserializeAsync<TwitchUserResponse>(content, options);
                if (data.Data.Count == 0)
                {
                    return null;
                }
                return data.Data.Single().Id;
            }
        }

        public async Task<string> GetTwitchDisplayName(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return null;

            if (userId.Length > 20) return null;

            string query = Uri.EscapeDataString(userId.Trim());

            string apiUrl = $"https://api.twitch.tv/helix/users?id={query}";
            if (!Uri.TryCreate(apiUrl, UriKind.Absolute, out Uri uri)) return null;

            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = uri;
                request.Headers.Add("Client-Id", _options.ClientId);
                request.Headers.Add("Authorization", $"Bearer {await GetAppAccessToken()}");
                var response = await _httpClient.SendAsync(request);

                // If token fails, retry once with a forced fresh token
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    request.Headers.Add("Authorization", $"Bearer {await GetAppAccessToken(true)}");
                    response = await _httpClient.SendAsync(request);
                }

                var content = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var data = await JsonSerializer.DeserializeAsync<TwitchUserResponse>(content, options);
                if (data.Data.Count == 0)
                {
                    return null;
                }
                return data.Data.Single().DisplayName;
            }
        }

        public class TwitchUserResponse
        {
            public List<TwitchUser> Data { get; set; }
        }

        public class TwitchUser
        {
            public string Id { get; set; }
            [JsonPropertyName("display_name")]
            public string DisplayName { get; set; }
        }

        private async Task<string> GetAppAccessToken(bool forceRefresh = false)
        {
            if (forceRefresh || !_memoryCache.TryGetValue(AppAccessTokenCacheKey, out string appAccessToken))
            {
                var freshToken = await GetFreshAppAccessToken();
                // Cache the token for slightly less than its lifetime
                _memoryCache.Set(AppAccessTokenCacheKey, freshToken.AccessToken, TimeSpan.FromSeconds(freshToken.ExpiresIn - 60));
                return freshToken.AccessToken;
            }

            return appAccessToken;
        }

        /// <summary>
        /// Get a fresh app access token from Twitch.
        /// https://dev.twitch.tv/docs/authentication/#app-access-tokens
        /// </summary>
        /// <returns>App access token response.</returns>
        private async Task<CredentialsGrantFlowResponse> GetFreshAppAccessToken()
        {
            // Implements the client credentials grant flow to get an app access token
            // https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#client-credentials-grant-flow
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri("https://id.twitch.tv/oauth2/token");

                var credentialsFlowData = new Dictionary<string, string>
                {
                    { "client_id", _options.ClientId },
                    { "client_secret", _options.ClientSecret },
                    { "grant_type", "client_credentials" }
                };

                request.Content = new FormUrlEncodedContent(credentialsFlowData);
                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStreamAsync();
                var data = await JsonSerializer.DeserializeAsync<CredentialsGrantFlowResponse>(content);
                return data;
            }
        }

        private class CredentialsGrantFlowResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }
            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }
            [JsonPropertyName("token_type")]
            public string TokenType { get; set; }
        }
    }

    public class TwitchServiceOptions
    {
        /// <summary>
        /// The Client ID, used to generate an App Access Token.
        /// https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#client-credentials-grant-flow
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The Client Secret, used to generate an App Access Token.
        /// https://dev.twitch.tv/docs/authentication/getting-tokens-oauth/#client-credentials-grant-flow
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
