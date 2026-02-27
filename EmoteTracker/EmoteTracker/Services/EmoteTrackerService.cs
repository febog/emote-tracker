using EmoteTracker.Data;
using EmoteTracker.Models;
using EmoteTracker.Services.EmoteProviders;
using EmoteTracker.Services.EmoteProviders.Bttv;
using EmoteTracker.Services.EmoteProviders.Franker;
using EmoteTracker.Services.EmoteProviders.Seven;
using EmoteTracker.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace EmoteTracker.Services
{
    public class EmoteTrackerService(EmoteTrackerContext context,
        HybridCache cache,
        ITwitchChannelService channelEmotesService,
        ITwitchService twitchService) : IEmoteTrackerService
    {
        private readonly EmoteTrackerContext _context = context;
        private readonly HybridCache _cache = cache;
        private readonly ITwitchChannelService _channelEmotesService = channelEmotesService;
        private readonly ITwitchService _twitchService = twitchService;

        private const string ChannelKey = "t";

        public async Task<TrackedChannel> GetChannelData(string channelName, bool forceRefresh = false, CancellationToken token = default)
        {
            if (forceRefresh)
            {
                await _cache.RemoveAsync($"{ChannelKey}:{channelName}", token);
            }

            return await _cache.GetOrCreateAsync(
                $"{ChannelKey}:{channelName}",
                async cancel => await GetChannelDataFromSource(channelName, forceRefresh),
                cancellationToken: token);
        }

        private async Task RefreshChannelEmotes(string channelId)
        {
            var trackedEmotes = await _channelEmotesService.GetChannelEmotes(channelId);

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
                        EmoteType = MapEmoteTypeToDatabase(emote.Provider),
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

        private static EmoteType MapEmoteTypeToDatabase(EmoteProvider type)
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

        private async Task<TrackedChannel> GetChannelDataFromSource(string channelName, bool forceRefresh = false)
        {
            var channelId = await _twitchService.GetTwitchId(channelName);

            if (channelId == null) return null;

            if (forceRefresh || !await _context.TwitchChannels.AnyAsync(c => c.Id == channelId))
            {
                await RefreshChannelEmotes(channelId);
            }

            var channelData = await _context.TwitchChannels
                    .Include(c => c.TwitchChannelEmotes.OrderBy(e => e.CanonicalName))
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == channelId);

            var emotes = channelData.TwitchChannelEmotes.Select(e => MapProviderEmoteToViewModel(MapDatabaseEmoteToProviderEmote(e)));

            return new TrackedChannel
            {
                ChannelId = channelId,
                DisplayName = channelData.DisplayName,
                TrackedEmotes = emotes.ToList(),
            };
        }

        private static IProviderEmote MapDatabaseEmoteToProviderEmote(TwitchChannelEmote e)
        {
            IProviderEmote emote;
            switch (e.EmoteType)
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
        }

        private static TrackedEmote MapProviderEmoteToViewModel(IProviderEmote e)
        {
            return new TrackedEmote
            {
                Id = e.Id,
                CanonicalName = e.CanonicalName,
                Alias = e.Alias,
                Width = e.Width,
                Height = e.Height,
                IsListed = e.IsListed,
                Type = MapEmoteTypeToViewModel(e.Provider),
                EmotePage = e.EmotePage,
                ImageUrl = e.ImageUrl,
            };
        }

        private static EmoteSource MapEmoteTypeToViewModel(EmoteProvider type)
        {
            switch (type)
            {
                case EmoteProvider.Unknown:
                    return EmoteSource.Unknown;
                case EmoteProvider.FrankerEmote:
                    return EmoteSource.FrankerEmote;
                case EmoteProvider.BttvEmote:
                    return EmoteSource.BttvEmote;
                case EmoteProvider.SevenEmote:
                    return EmoteSource.SevenEmote;
                default:
                    return EmoteSource.Unknown;
            }
        }
    }
}
