using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmoteTracker.Models
{
    [PrimaryKey(nameof(EmoteId), nameof(TwitchChannelId))]
    public class TwitchChannelEmote
    {
        [Key, Column(Order = 1)]
        [StringLength(30)]
        public string EmoteId { get; set; }

        [Key, Column(Order = 2)]
        [StringLength(20)]
        public string TwitchChannelId { get; set; }

        [Required]
        [StringLength(255)]
        public string CanonicalName { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsListed { get; set; }

        [StringLength(255)]
        public string Alias { get; set; }

        public EmoteType EmoteType { get; set; }

        public TwitchChannel TwitchChannel { get; set; }
    }

    public enum EmoteType
    {
        // Do not reorder
        Other = 0,
        FrankerEmote = 1,
        BttvEmote = 2,
        SevenEmote = 3
    }
}
