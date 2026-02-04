namespace EmoteTracker.Services.EmoteProviders
{
    public abstract class ProviderEmote : IProviderEmote
    {
        public string Id { get; set; }

        public string CanonicalName { get; set; }

        public string Alias { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsListed { get; set; }

        public abstract EmoteProvider Provider { get; }

        public abstract string EmotePage { get; }

        public abstract string ImageUrl { get; }
    }
}
