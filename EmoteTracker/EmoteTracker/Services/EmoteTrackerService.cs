using EmoteTracker.Data;
using EmoteTracker.Models;
using EmoteTracker.Services.EmoteProviders;
using EmoteTracker.Services.EmoteProviders.Bttv;
using EmoteTracker.Services.EmoteProviders.Franker;
using EmoteTracker.Services.EmoteProviders.Seven;
using Microsoft.EntityFrameworkCore;

namespace EmoteTracker.Services
{
    public class EmoteTrackerService(EmoteTrackerContext context,
        IFrankerService frankerService,
        IBttvService bttvService,
        ISevenService sevenService,
        ITwitchService twitchService) : IEmoteTrackerService
    {
        private readonly EmoteTrackerContext _context = context;
        private readonly IFrankerService _frankerService = frankerService;
        private readonly IBttvService _bttvService = bttvService;
        private readonly ISevenService _sevenService = sevenService;
        private readonly ITwitchService _twitchService = twitchService;

        public async Task RefreshChannelEmotes(string channelId)
        {
            var frankerEmotes = await _frankerService.GetProviderEmotes(channelId);
            if (frankerEmotes == null) throw new ArgumentException("Franker emotes could not be loaded.");

            var bttvEmotes = await _bttvService.GetProviderEmotes(channelId);
            if (bttvEmotes == null) throw new ArgumentException("BTTV emotes could not be loaded.");

            var sevenEmotes = await _sevenService.GetProviderEmotes(channelId);
            if (sevenEmotes == null) throw new ArgumentException("SevenTV emotes could not be loaded.");

            var trackedEmotes = frankerEmotes.Concat(bttvEmotes).Concat(sevenEmotes);

            var channelToUpdate = await _context.TwitchChannels
                .Include(c => c.TwitchChannelEmotes)
                .FirstOrDefaultAsync(c => c.Id == channelId);

            if (channelToUpdate == null)
            {
                var emptyTwitchChannel = new TwitchChannel();
                emptyTwitchChannel.Id = channelId;
                emptyTwitchChannel.DisplayName = await _twitchService.GetTwitchDisplayName(channelId);
                emptyTwitchChannel.TwitchChannelEmotes = new List<TwitchChannelEmote>();
                _context.TwitchChannels.Add(emptyTwitchChannel);

                channelToUpdate = emptyTwitchChannel;
            }

            // Add new found emotes

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
                        EmoteType = MapEmoteType(emote.Provider),
                    });
                }
            }

            // Delete removed emotes

            var trackedEmotesHS = new HashSet<string>(trackedEmotes.Select(e => e.Id));

            foreach (var emote in channelToUpdate.TwitchChannelEmotes)
            {
                if (!trackedEmotesHS.Contains(emote.EmoteId))
                {
                    var emoteToRemove = channelToUpdate.TwitchChannelEmotes.Single(e => e.EmoteId == emote.EmoteId);
                    channelToUpdate.TwitchChannelEmotes.Remove(emoteToRemove);
                }
            }

            await _context.SaveChangesAsync();
        }

        private static EmoteType MapEmoteType(EmoteProvider type)
        {
            switch (type)
            {
                case EmoteProvider.Unknown:
                    return EmoteType.Other;
                case EmoteProvider.FrankerEmote:
                    return EmoteType.FrankerEmote;
                case EmoteProvider.BttvEmote:
                    return EmoteType.BttvEmote;
                case EmoteProvider.SevenEmote:
                    return EmoteType.SevenEmote;
                default:
                    return EmoteType.Other;
            }
        }

        public async Task<List<IProviderEmote>> GetChannelEmotes(string channelId, bool forceRefresh = false)
        {
            var channelData = await _context.TwitchChannels
                    .Include(c => c.TwitchChannelEmotes.OrderBy(e => e.CanonicalName))
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == channelId);

            if (channelData == null || forceRefresh)
            {
                await RefreshChannelEmotes(channelId);
                channelData = await _context.TwitchChannels
                    .Include(c => c.TwitchChannelEmotes.OrderBy(e => e.CanonicalName))
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == channelId);
            }

            var emotes = new List<IProviderEmote>(channelData.TwitchChannelEmotes.Count);

            emotes.AddRange(channelData.TwitchChannelEmotes.Select(e =>
            {
                IProviderEmote emote;
                switch(e.EmoteType)
                {
                    case EmoteType.FrankerEmote:
                        emote = new FrankerEmote
                        {
                            Id = e.EmoteId,
                            CanonicalName = e.CanonicalName,
                            Width = e.Width,
                            Height = e.Height,
                            IsListed = e.IsListed,
                        };
                        break;
                    case EmoteType.BttvEmote:
                        emote = new BttvEmote
                        {
                            Id = e.EmoteId,
                            CanonicalName = e.CanonicalName,
                            Width = e.Width,
                            Height = e.Height,
                            IsListed = e.IsListed,
                        };
                        break;
                    case EmoteType.SevenEmote:
                        emote = new SevenEmote
                        {
                            Id = e.EmoteId,
                            CanonicalName = e.CanonicalName,
                            Alias = e.Alias,
                            Width = e.Width,
                            Height = e.Height,
                            IsListed = e.IsListed,
                        };
                        break;
                    default:
                        throw new ArgumentException("Emote type error.");
                }
                return emote;
            }));

            return emotes;
        }
    }
}
