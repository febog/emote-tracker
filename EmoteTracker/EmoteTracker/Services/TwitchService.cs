using EmoteTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmoteTracker.Services
{
    public class TwitchService : ITwitchService
    {
        private readonly HttpClient _httpClient;
        private readonly TwitchServiceOptions _options;
        private readonly EmoteTrackerContext _context;

        public TwitchService(HttpClient httpClient, IOptions<TwitchServiceOptions> options, EmoteTrackerContext context)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _context = context;
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

        public class TwitchUserResponse
        {
            public List<TwitchUser> Data { get; set; }
        }

        public class TwitchUser
        {
            public string Id { get; set; }
        }

        private async Task<string> GetAppAccessToken(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                var freshToken = await GetFreshAppAccessToken();
                var tokenToRefresh = await _context.TwitchAppAccessTokens.SingleAsync(x => x.TokenType == "bearer");
                tokenToRefresh.AccessToken = freshToken.AccessToken;
                tokenToRefresh.ExpiresIn = freshToken.ExpiresIn;
                tokenToRefresh.TokenType = freshToken.TokenType;
                await _context.SaveChangesAsync();
                return tokenToRefresh.AccessToken;
            }

            return (await _context.TwitchAppAccessTokens.SingleAsync(x => x.TokenType == "bearer")).AccessToken;
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
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<CredentialsGrantFlowResponse>(content);
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
