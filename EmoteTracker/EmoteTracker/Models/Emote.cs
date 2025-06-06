using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EmoteTracker.Models
{
    public class Emote
    {
        [Key]
        [StringLength(30)]
        [Display(Name = "EMOTE_ID")]
        public string Id { get; set; }

        public int EmoteServiceId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "EMOTE_CANONICAL_NAME")]
        public string CanonicalName { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsListed { get; set; }

        [Display(Name = "EMOTE_EMOTE_SERVICE")]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public EmoteService EmoteService { get; set; }
    }
}
