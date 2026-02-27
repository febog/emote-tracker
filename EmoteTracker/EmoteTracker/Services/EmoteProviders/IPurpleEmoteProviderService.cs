namespace EmoteTracker.Services.EmoteProviders
{
    public interface IPurpleEmoteProviderService
    {
        /// <summary>
        /// Gets all the enabled channel emotes from a given emote provider service.
        /// </summary>
        /// <param name="channelId">Twitch channel Id to get emotes for.</param>
        /// <param name="token">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>All the enabled emotes for this channel.</returns>
        Task<IEnumerable<IProviderEmote>> GetProviderEmotes(string channelId, CancellationToken token = default);
    }
}
