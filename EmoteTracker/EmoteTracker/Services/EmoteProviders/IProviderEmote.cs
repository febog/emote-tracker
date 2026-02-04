using System.ComponentModel.DataAnnotations;

namespace EmoteTracker.Services.EmoteProviders
{
    public interface IProviderEmote
    {
        string Id { get; }

        [Display(Name = "Name")]
        string CanonicalName { get; }

        string Alias { get; }

        int Width { get; }

        int Height { get; }

        bool IsListed { get; }

        [Display(Name = "Type")]
        public EmoteProvider Provider { get; }

        string EmotePage { get; }

        string ImageUrl { get; }
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
