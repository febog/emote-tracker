namespace EmoteTracker.Services.EmoteProviders
{
    public interface IEmoteProviderService
    {
        Task<List<IProviderEmote>> GetChannelEmotes(string channelId);
    }
}
