using System.ComponentModel.DataAnnotations;

namespace EmoteTracker.Models
{
    public class TwitchAppAccessToken
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string AccessToken { get; set; }

        public int ExpiresIn { get; set; }

        [Required]
        [StringLength(10)]
        public string TokenType { get; set; }
    }
}
