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
        FrankerEmote = 0,
        BttvEmote = 1,
        SevenEmote = 2
    }
}
