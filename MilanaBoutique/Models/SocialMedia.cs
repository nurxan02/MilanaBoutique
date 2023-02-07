using System.ComponentModel.DataAnnotations;

namespace MilanaBoutique.Models
{
    public class SocialMedia
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 165)]
        public string Icon { get; set; }

        [Required]
        [StringLength(maximumLength: 165)]
        public string Link { get; set; }
        public Member Member { get; set; }

        public int MemberId { get; set; }
    }
}
