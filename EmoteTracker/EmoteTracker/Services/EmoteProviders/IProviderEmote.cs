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

        public EmoteProvider Provider { get; }

        string EmotePage { get; }

        string ImageUrl { get; }
    }

    public enum EmoteProvider
    {
        // Do not reorder
        Unknown = 0,
        FrankerEmote = 1,
        BttvEmote = 2,
        SevenEmote = 3
    }
}
