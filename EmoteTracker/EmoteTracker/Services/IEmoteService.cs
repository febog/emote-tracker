namespace EmoteTracker.Services
{
    public interface IEmoteService
    {
        Task<List<ChannelEmote>> GetChannelEmotes(string channelId);
    }
}
