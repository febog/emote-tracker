using System.Text.Json;

namespace EmoteTracker.Services
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

        public async Task<List<ChannelEmote>> GetChannelEmotes(string channelId)
        {
            if (string.IsNullOrWhiteSpace(channelId)) return null;

            var response = await _httpClient.GetAsync(channelId);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return [];
            var content = await response.Content.ReadAsStreamAsync();
            using (var document = await JsonDocument.ParseAsync(content))
            {
                var root = document.RootElement;
                var emotes = root.GetProperty("emote_set").GetProperty("emotes");

                var sevenEmotes = emotes.EnumerateArray().Select(e =>
                {
                    var alias = e.GetProperty("name").ToString();
                    var canonicalName = e.GetProperty("data").GetProperty("name").ToString();
                    var emoteIsAliased = alias != canonicalName;

                    // Get first file element for smallest
                    var fileData = e.GetProperty("data").GetProperty("host").GetProperty("files")
                    .EnumerateArray().First();

                    return new SevenEmote
                    {
                        Id = e.GetProperty("id").ToString(),
                        CanonicalName = canonicalName,
                        Alias = emoteIsAliased ? alias : null,
                        Width = fileData.GetProperty("width").GetInt32(),
                        Height = fileData.GetProperty("height").GetInt32(),
                        IsListed = true,
                        EmoteType = ChannelEmoteType.SevenEmote,
                    };
                }).ToList();

                // Turns out the 7TV API sometimes returns duplicate IDs i.e. the same emote more than once
                // Remove duplicates
                var seenIds = new HashSet<string>();
                var result = new List<ChannelEmote>(sevenEmotes.Count);

                foreach (var emote in sevenEmotes)
                {
                    if (seenIds.Add(emote.Id)) // Add returns false if already present
                    {
                        result.Add(emote);
                    }
                }

                return result;
            }
        }
    }
}
