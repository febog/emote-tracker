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
            var response = await _httpClient.GetAsync(channelId);
            var content = await response.Content.ReadAsStreamAsync();
            using (var document = await JsonDocument.ParseAsync(content))
            {
                var root = document.RootElement;
                var emotes = root.GetProperty("emote_set").GetProperty("emotes");

                return emotes.EnumerateArray().Select(e =>
                {
                    var alias = e.GetProperty("name").ToString();
                    var canonicalName = e.GetProperty("data").GetProperty("name").ToString();
                    var emoteIsAliased = alias != canonicalName;

                    // Get first file element for smallest
                    var fileData = e.GetProperty("data").GetProperty("host").GetProperty("files")
                    .EnumerateArray().First();

                    return new ChannelEmote
                    {
                        Id = e.GetProperty("id").ToString(),
                        CanonicalName = canonicalName,
                        Alias = emoteIsAliased ? alias : null,
                        Width = fileData.GetProperty("width").GetInt32(),
                        Height = fileData.GetProperty("height").GetInt32(),
                        IsListed = true
                    };
                }).ToList();
            }
        }
    }
}
