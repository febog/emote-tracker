using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmoteTracker.Models
{
    public class TwitchChannelEmote
    {
        [Key, Column(Order = 1)]
        [StringLength(30)]
        public string EmoteId { get; set; }

        [Key, Column(Order = 2)]
        [StringLength(20)]
        public string TwitchChannelId { get; set; }

        [StringLength(255)]
        public string Alias { get; set; }

        public Emote Emote { get; set; }

        public TwitchChannel TwitchChannel { get; set; }
    }
}
