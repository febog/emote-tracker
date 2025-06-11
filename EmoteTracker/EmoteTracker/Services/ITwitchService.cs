namespace EmoteTracker.Services
{
    /// <summary>
    /// A minimal Twitch service for a few API calls that only require app access token authorization
    /// </summary>
    public interface ITwitchService
    {
        /// <summary>
        /// Returns the Twitch Id of the given username. Null if none is found.
        /// </summary>
        /// <param name="username">The login name of the user to get.</param>
        /// <returns></returns>
        Task<string> GetTwitchId(string username);

        /// <summary>
        /// Returns the Twitch display name for the given Twitch Id. Null if none is found.
        /// </summary>
        /// <param name="userId">The Id of the user to get.</param>
        /// <returns></returns>
        Task<string> GetTwitchDisplayName(string userId);
    }
}
