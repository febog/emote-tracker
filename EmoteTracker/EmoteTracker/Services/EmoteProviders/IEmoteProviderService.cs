namespace EmoteTracker.Services.EmoteProviders
{
    public interface IEmoteProviderService
    {
        Task<List<ChannelEmote>> GetChannelEmotes(string channelId);
    }
}
