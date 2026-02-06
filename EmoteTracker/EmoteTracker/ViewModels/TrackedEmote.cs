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

        [Display(Name = "Type")]
        public EmoteProvider Provider { get; }

        public string EmotePage { get; }

        public string ImageUrl { get; }
    }

    public enum EmoteProvider
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
