using System.Text.Json;

namespace EmoteTracker.Services
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

        public async Task<List<ChannelEmote>> GetChannelEmotes(string channelId)
        {
            if (string.IsNullOrWhiteSpace(channelId)) return null;

            var response = await _httpClient.GetAsync(channelId);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) return [];
            var content = await response.Content.ReadAsStreamAsync();
            using (var document = await JsonDocument.ParseAsync(content))
            {
                var root = document.RootElement;
                var setId = root.GetProperty("room").GetProperty("set").ToString();
                var emotes = root.GetProperty("sets").GetProperty(setId).GetProperty("emoticons");

                return emotes.EnumerateArray().Select(e => new ChannelEmote
                {
                    Id = e.GetProperty("id").ToString(),
                    CanonicalName = e.GetProperty("name").ToString(),
                    Width = e.GetProperty("width").GetInt32(),
                    Height = e.GetProperty("height").GetInt32(),
                    IsListed = true,
                    EmoteType = ChannelEmoteType.FrankerEmote,
                }).ToList();
            }
        }
    }
}
