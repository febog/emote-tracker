using EmoteTracker.Services.EmoteProviders;

namespace EmoteTracker.Services
{
    public interface IEmoteTrackerService
    {
        Task RefreshChannelEmotes(string channelId);
        Task<List<IProviderEmote>> GetChannelEmotes(string channelId, bool forceRefresh = false);
    }
}
