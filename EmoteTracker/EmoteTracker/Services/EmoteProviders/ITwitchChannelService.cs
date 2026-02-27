namespace EmoteTracker.Services.EmoteProviders
{
    public interface ITwitchChannelService
    {
        Task<IEnumerable<IProviderEmote>> GetChannelEmotes(string channelId);
    }
}
