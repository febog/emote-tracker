using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmoteTracker.Services.EmoteProviders.Bttv
{
    public class BttvService : IBttvService
    {
        // BTTV (https://betterttv.com/)
        // API Documentation: https://betterttv.com/developers/api#user
        // Base URL: https://api.betterttv.net/3/cached/users/twitch/

        private readonly HttpClient _httpClient;

        public BttvService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<IProviderEmote>> GetProviderEmotes(string channelId)
        {
            if (string.IsNullOrWhiteSpace(channelId)) return null;

            var response = await _httpClient.GetAsync(channelId);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return [];
            var content = await response.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<BttvResponse>(content);
            var bttvEmotes = data.ChannelEmotes.Concat(data.SharedEmotes).Select(e => new BttvEmote
            {
                Id = e.Id,
                CanonicalName = e.Code,
                Width = 28,
                Height = 28,
                IsListed = true,
            });

            return bttvEmotes;
        }

        private class BttvResponse
        {
            [JsonPropertyName("channelEmotes")]
            public List<BttvEmoticon> ChannelEmotes { get; set; }

            [JsonPropertyName("sharedEmotes")]
            public List<BttvEmoticon> SharedEmotes { get; set; }
        }

        private class BttvEmoticon
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("code")]
            public string Code { get; set; }

        }
    }
}
