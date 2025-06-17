using System.ComponentModel.DataAnnotations;

namespace EmoteTracker.Services
{
    public abstract class ChannelEmote
    {
        public string Id { get; set; }

        public string CanonicalName { get; set; }

        public string Alias { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsListed { get; set; }

        public abstract ChannelEmoteType EmoteType { get; }

        public abstract string EmotePage { get; }

        public abstract string ImageUrl { get; }
    }

    public enum ChannelEmoteType
    {
        // Do not reorder
        Other = 0,
        [Display(Name = "FFZ")]
        FrankerEmote = 1,
        [Display(Name = "BTTV")]
        BttvEmote = 2,
        [Display(Name = "7TV")]
        SevenEmote = 3
    }
}
