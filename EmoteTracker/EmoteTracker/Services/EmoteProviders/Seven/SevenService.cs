using EmoteTracker.Services.EmoteProviders.Franker;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmoteTracker.Services.EmoteProviders.Seven
{
    public class SevenService : ISevenService
    {
        // 7TV (https://7tv.app/)
        // API Documentation: https://7tv.io/docs ("Get User Connection")
        // Base URL: https://7tv.io/v3/users/twitch/

        private readonly HttpClient _httpClient;

        public SevenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<IProviderEmote>> GetChannelEmotes(string channelId)
        {
            if (string.IsNullOrWhiteSpace(channelId)) return null;

            var response = await _httpClient.GetAsync(channelId);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return [];
            var content = await response.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<SevenResponse>(content);
            var sevenEmotes = data.EmoteSet.Emotes.Select(e =>
            {
                var nameInChat = e.Name;
                var canonincalName = e.Data.CanonicalName;
                var emoteIsAliased = nameInChat != canonincalName;

                // The first emote file is the smallest, I will take that one
                var emoteFile = e.Data.Host.Files.First();

                return new SevenEmote
                {
                    Id = e.Id,
                    CanonicalName = canonincalName,
                    Alias = emoteIsAliased ? nameInChat : null,
                    Width = emoteFile.Width,
                    Height = emoteFile.Height,
                    IsListed = e.Data.Listed,
                };
            });

            // Turns out the 7TV API sometimes returns duplicate IDs i.e. the same emote more than once
            // Remove duplicates
            var unique = sevenEmotes.DistinctBy(e => e.Id);

            return unique;
        }

        private class SevenResponse
        {
            [JsonPropertyName("emote_set")]
            public SevenEmoteSet EmoteSet { get; set; }
        }

        private class SevenEmoteSet
        {
            [JsonPropertyName("emotes")]
            public List<SevenEmoticon> Emotes { get; set; }
        }

        private class SevenEmoticon
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("data")]
            public EmoteData Data { get; set; }
        }

        private class EmoteData
        {
            [JsonPropertyName("name")]
            public string CanonicalName { get; set; }

            [JsonPropertyName("listed")]
            public bool Listed { get; set; }

            [JsonPropertyName("host")]
            public EmoteHost Host { get; set; }
        }

        private class EmoteHost
        {
            [JsonPropertyName("files")]
            public List<EmoteFile> Files { get; set; }
        }

        private class EmoteFile
        {
            [JsonPropertyName("width")]
            public int Width { get; set; }

            [JsonPropertyName("height")]
            public int Height { get; set; }
        }
    }
}
