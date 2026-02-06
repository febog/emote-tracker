using System.ComponentModel.DataAnnotations;

namespace EmoteTracker.ViewModels
{
    public class TrackedChannel
    {
        [Display(Name = "TWITCH_CHANNEL_ID")]
        public string ChannelId { get; set; }

        [Display(Name = "TWITCH_CHANNEL_DISPLAY_NAME")]
        public string DisplayName { get; set; }

        public IList<TrackedEmote> TrackedEmotes { get; set; }
    }
}
