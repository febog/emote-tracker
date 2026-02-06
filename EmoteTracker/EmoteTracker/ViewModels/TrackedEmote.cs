using System.ComponentModel.DataAnnotations;

namespace EmoteTracker.ViewModels
{
    public class TrackedEmote
    {
        public string Id { get; set; }

        [Display(Name = "Name")]
        public string CanonicalName { get; set; }

        public string Alias { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsListed { get; set; }

        public EmoteSource Type { get; set; }

        public string EmotePage { get; set; }

        public string ImageUrl { get; set; }
    }

    public enum EmoteSource
    {
        // Do not reorder
        Unknown = 0,
        [Display(Name = "FFZ")]
        FrankerEmote = 1,
        [Display(Name = "BTTV")]
        BttvEmote = 2,
        [Display(Name = "7TV")]
        SevenEmote = 3
    }
}
