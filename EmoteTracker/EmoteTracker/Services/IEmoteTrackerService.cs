using EmoteTracker.ViewModels;

namespace EmoteTracker.Services
{
    public interface IEmoteTrackerService
    {
        Task<TrackedChannel> GetChannelData(string channelName, bool forceRefresh = false, CancellationToken token = default);
    }
}
