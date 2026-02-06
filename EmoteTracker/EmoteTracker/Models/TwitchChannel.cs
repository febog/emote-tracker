using System.ComponentModel.DataAnnotations;

namespace EmoteTracker.Models
{
    public class TwitchChannel
    {
        [Key]
        [StringLength(20)]
        public string Id { get; set; }

        [Required]
        [StringLength(25)]
        public string DisplayName { get; set; }

        public virtual ICollection<TwitchChannelEmote> TwitchChannelEmotes { get; set; }
    }
}
