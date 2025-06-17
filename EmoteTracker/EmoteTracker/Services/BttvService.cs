using System.Text.Json;

namespace EmoteTracker.Services
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

        public async Task<List<ChannelEmote>> GetChannelEmotes(string channelId)
        {
            if (string.IsNullOrWhiteSpace(channelId)) return null;

            var response = await _httpClient.GetAsync(channelId);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return [];
            var content = await response.Content.ReadAsStreamAsync();
            using (var document = await JsonDocument.ParseAsync(content))
            {
                var root = document.RootElement;
                // This API returns the chat emotes in 2 sets: "channel" and "shared". Image dimensions taken empirically from website. Not explicitly set in API.
                var emotes = root.GetProperty("channelEmotes").EnumerateArray().Select(e => (ChannelEmote)new BttvEmote
                {
                    Id = e.GetProperty("id").ToString(),
                    CanonicalName = e.GetProperty("code").ToString(),
                    Width = 38,
                    Height = 28,
                    IsListed = true,
                }).ToList();

                emotes.AddRange(root.GetProperty("sharedEmotes").EnumerateArray().Select(e => (ChannelEmote)new BttvEmote
                {
                    Id = e.GetProperty("id").ToString(),
                    CanonicalName = e.GetProperty("code").ToString(),
                    Width = 38,
                    Height = 28,
                    IsListed = true,
                    EmoteType = ChannelEmoteType.BttvEmote,
                }).ToList());

                return emotes;
            }
        }
    }
}
