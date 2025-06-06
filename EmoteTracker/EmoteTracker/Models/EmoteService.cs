using System.ComponentModel.DataAnnotations;

namespace EmoteTracker.Models
{
    public class EmoteService
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public virtual ICollection<Emote> Emotes { get; set; }
    }
}
