using EmoteTracker.Services.EmoteProviders.Bttv;
using EmoteTracker.Services.EmoteProviders.Franker;
using EmoteTracker.Services.EmoteProviders.Seven;

namespace EmoteTracker.Services.EmoteProviders
{
    public class TwitchChannelService : ITwitchChannelService
    {
        private readonly IReadOnlyCollection<IPurpleEmoteProviderService> _emoteProviders;

        public TwitchChannelService(
            IBttvService bttvService,
            IFrankerService frankerService,
            ISevenService sevenService)
        {
            _emoteProviders = [bttvService, frankerService, sevenService];
        }

        public async Task<IEnumerable<IProviderEmote>> GetChannelEmotes(string channelId, CancellationToken token = default)
        {
            var results = await Task.WhenAll(_emoteProviders.Select(s => s.GetProviderEmotes(channelId, token)));
            var channelEmotes = results.SelectMany(emotes => emotes);

            return channelEmotes;
        }
    }
}
