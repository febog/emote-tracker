namespace EmoteTracker.Services
{
    public interface IEmoteTrackerService
    {
        Task RefreshChannelEmotes(string channelId);
    }
}
