using EmoteTracker.Data;
using EmoteTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace EmoteTracker.Services
{
    public class EmoteTrackerService(EmoteTrackerContext context,
        IFrankerService frankerService,
        IBttvService bttvService,
        ISevenService sevenService) : IEmoteTrackerService
    {
        private readonly EmoteTrackerContext _context = context;
        private readonly IFrankerService _frankerService = frankerService;
        private readonly IBttvService _bttvService = bttvService;
        private readonly ISevenService _sevenService = sevenService;

        public async Task RefreshChannelEmotes(string channelId)
        {
            var frankerEmotes = await _frankerService.GetChannelEmotes(channelId);
            if (frankerEmotes == null) throw new ArgumentException("Franker emotes could not be loaded.");

            var bttvEmotes = await _bttvService.GetChannelEmotes(channelId);
            if (bttvEmotes == null) throw new ArgumentException("BTTV emotes could not be loaded.");

            var sevenEmotes = await _sevenService.GetChannelEmotes(channelId);
            if (sevenEmotes == null) throw new ArgumentException("SevenTV emotes could not be loaded.");

            var trackedEmotes = new List<ChannelEmote>(
                frankerEmotes.Count +
                bttvEmotes.Count +
                sevenEmotes.Count);

            trackedEmotes.AddRange(frankerEmotes);
            trackedEmotes.AddRange(bttvEmotes);
            trackedEmotes.AddRange(sevenEmotes);

            var channelToUpdate = await _context.TwitchChannels
                .Include(c => c.TwitchChannelEmotes)
                .FirstOrDefaultAsync();

            if (channelToUpdate == null)
            {
                var emptyTwitchChannel = new TwitchChannel();
                emptyTwitchChannel.Id = channelId;
                emptyTwitchChannel.DisplayName = "foo";
                _context.TwitchChannels.Add(emptyTwitchChannel);
                await _context.SaveChangesAsync();

                channelToUpdate = await _context.TwitchChannels
                    .Include(c => c.TwitchChannelEmotes)
                    .FirstOrDefaultAsync();
            }

            var channelEmotesHS = new HashSet<string>(channelToUpdate.TwitchChannelEmotes
                .Select(e => e.EmoteId));

            foreach (var emote in trackedEmotes)
            {
                if (!channelEmotesHS.Contains(emote.Id))
                {
                    channelToUpdate.TwitchChannelEmotes.Add(new TwitchChannelEmote
                    {
                        EmoteId = emote.Id,
                        CanonicalName = emote.CanonicalName,
                        Alias = emote.Alias,
                        Width = emote.Width,
                        Height = emote.Height,
                        IsListed = emote.IsListed,
                        EmoteType = MapEmoteType(emote.EmoteType),
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        private static EmoteType MapEmoteType(ChannelEmoteType type)
        {
            switch (type)
            {
                case ChannelEmoteType.Other:
                    return EmoteType.Other;
                case ChannelEmoteType.FrankerEmote:
                    return EmoteType.FrankerEmote;
                case ChannelEmoteType.BttvEmote:
                    return EmoteType.BttvEmote;
                case ChannelEmoteType.SevenEmote:
                    return EmoteType.SevenEmote;
                default:
                    return EmoteType.Other;
            }
        }
    }
}
