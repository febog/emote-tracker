namespace EmoteTracker.Services
{
    public class ChannelEmote
    {
        public string Id { get; set; }

        public string CanonicalName { get; set; }

        public string Alias { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsListed { get; set; }

        public ChannelEmoteType EmoteType { get; set; }
    }

    public enum ChannelEmoteType
    {
        // Do not reorder
        Other = 0,
        FrankerEmote = 1,
        BttvEmote = 2,
        SevenEmote = 3
    }
}
