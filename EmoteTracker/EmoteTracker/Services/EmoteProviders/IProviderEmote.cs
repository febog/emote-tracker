namespace EmoteTracker.Services.EmoteProviders
{
    public interface IProviderEmote
    {
        string Id { get; }

        string CanonicalName { get; }

        string Alias { get; }

        int Width { get; }

        int Height { get; }

        bool IsListed { get; }

        public ChannelEmoteType EmoteType { get; }

        string EmotePage { get; }

        string ImageUrl { get; }
    }
}
