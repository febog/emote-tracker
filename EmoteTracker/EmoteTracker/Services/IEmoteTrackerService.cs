namespace EmoteTracker.Services
{
    public interface IEmoteTrackerService
    {
        Task RefreshChannelEmotes(string channelId);
        Task<List<ChannelEmote>> GetChannelEmotes(string channelId);
    }
}
