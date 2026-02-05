namespace EmoteTracker.Services.EmoteProviders
{
    public interface IChannelEmotesService
    {
        Task<IEnumerable<IProviderEmote>> GetChannelEmotes(string channelId);
    }
}
