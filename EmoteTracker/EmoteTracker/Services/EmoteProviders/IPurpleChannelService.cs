namespace EmoteTracker.Services.EmoteProviders
{
    public interface IPurpleChannelService
    {
        Task<IEnumerable<IProviderEmote>> GetChannelEmotes(string channelId);
    }
}
