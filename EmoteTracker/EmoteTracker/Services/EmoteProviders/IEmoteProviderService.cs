namespace EmoteTracker.Services.EmoteProviders
{
    public interface IEmoteProviderService
    {
        Task<IEnumerable<IProviderEmote>> GetChannelEmotes(string channelId);
    }
}
