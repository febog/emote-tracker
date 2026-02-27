namespace EmoteTracker.Services.EmoteProviders
{
    public interface IPurpleEmoteProviderService
    {
        /// <summary>
        /// Gets all the enabled channel emotes from a given emote provider service.
        /// </summary>
        /// <param name="channelId">Twitch channel Id to get emotes for.</param>
        /// <returns>All the enabled emotes for this channel.</returns>
        Task<IEnumerable<IProviderEmote>> GetProviderEmotes(string channelId);
    }
}
