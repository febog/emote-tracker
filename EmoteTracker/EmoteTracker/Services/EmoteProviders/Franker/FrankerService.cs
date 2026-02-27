using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmoteTracker.Services.EmoteProviders.Franker
{
    public class FrankerService : IFrankerService
    {
        // FFZ (https://www.frankerfacez.com/)
        // API Documentation: https://api.frankerfacez.com/docs/#/Rooms/get_v1_room_id__twitchID_
        // Base URL: https://api.frankerfacez.com/v1/room/id/

        private readonly HttpClient _httpClient;

        public FrankerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<IProviderEmote>> GetProviderEmotes(string channelId, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(channelId)) return null;

            var response = await _httpClient.GetAsync(channelId, token);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return [];
            var content = await response.Content.ReadAsStreamAsync(token);
            var data = await JsonSerializer.DeserializeAsync<FrankerResponse>(content, cancellationToken: token);
            var setId = data.Room.Set;
            var frankerEmotes = data.Sets[setId.ToString()].Emoticons.Select(e =>
            {
                return new FrankerEmote
                {
                    Id = e.Id.ToString(),
                    CanonicalName = e.Name,
                    Width = e.Width,
                    Height = e.Height,
                    IsListed = true,
                };
            });

            return frankerEmotes;
        }

        private class FrankerResponse
        {
            [JsonPropertyName("room")]
            public FrankerRoom Room { get; set; }

            [JsonPropertyName("sets")]
            public Dictionary<string, FrankerSet> Sets { get; set; }
        }

        private class FrankerRoom
        {
            [JsonPropertyName("set")]
            public int Set { get; set; }
        }

        private class FrankerSet
        {
            [JsonPropertyName("emoticons")]
            public List<FrankerEmoticon> Emoticons { get; set; }
        }

        private class FrankerEmoticon
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("width")]
            public int Width { get; set; }

            [JsonPropertyName("height")]
            public int Height { get; set; }
        }
    }
}
