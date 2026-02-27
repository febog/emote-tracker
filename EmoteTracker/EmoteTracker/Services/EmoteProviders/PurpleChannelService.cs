using EmoteTracker.Services.EmoteProviders.Bttv;
using EmoteTracker.Services.EmoteProviders.Franker;
using EmoteTracker.Services.EmoteProviders.Seven;

namespace EmoteTracker.Services.EmoteProviders
{
    public class PurpleChannelService : IPurpleChannelService
    {
        private readonly IReadOnlyCollection<IPurpleEmoteProviderService> _emoteProviders;

        public PurpleChannelService(
            IBttvService bttvService,
            IFrankerService frankerService,
            ISevenService sevenService)
        {
            _emoteProviders = [bttvService, frankerService, sevenService];
        }

        public async Task<IEnumerable<IProviderEmote>> GetChannelEmotes(string channelId)
        {
            var results = await Task.WhenAll(_emoteProviders.Select(s => s.GetProviderEmotes(channelId)));
            var channelEmotes = results.SelectMany(emotes => emotes);

            return channelEmotes;
        }
    }
}
