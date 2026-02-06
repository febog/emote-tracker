using EmoteTracker.ViewModels;

namespace EmoteTracker.Services
{
    public interface IEmoteTrackerService
    {
        Task RefreshChannelEmotes(string channelId);
        Task<TrackedChannel> GetChannelData(string channelName, bool forceRefresh = false);
    }
}
