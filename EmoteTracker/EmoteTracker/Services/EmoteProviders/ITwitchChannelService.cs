namespace EmoteTracker.Services.EmoteProviders
{
    public interface ITwitchChannelService
    {
        /// <summary>
        /// Gets all the enabled emotes from a given Twitch channel.
        /// </summary>
        /// <param name="channelId">Twitch channel Id to get emotes for.</param>
        /// <param name="token">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>All the enabled emotes for this channel.</returns>
        Task<IEnumerable<IProviderEmote>> GetChannelEmotes(string channelId, CancellationToken token = default);
    }
}
