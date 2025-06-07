using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace EmoteTracker.Services
{
    public class TwitchService : ITwitchService
    {
        private readonly HttpClient _httpClient;
        private readonly TwitchServiceOptions _options;

        private const string MyAppAccessToken = "sn3k5tkzc2pgu6b6t2aandzkbx31hq";

        public TwitchService(HttpClient httpClient, IOptions<TwitchServiceOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
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
                request.Headers.Add("Authorization", $"Bearer {MyAppAccessToken}");
                var response = await _httpClient.SendAsync(request);
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
