using System.ComponentModel.DataAnnotations;

namespace EmoteTracker.Models
{
    public class TwitchChannel
    {
        [Key]
        [StringLength(20)]
        [Display(Name = "TWITCH_CHANNEL_ID")]
        public string Id { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "TWITCH_CHANNEL_DISPLAY_NAME")]
        public string DisplayName { get; set; }

        public virtual ICollection<TwitchChannelEmote> TwitchChannelEmotes { get; set; }
    }
}
