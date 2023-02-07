using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilanaBoutique.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 150)]
        [Required]
        public string TextSmall { get; set; }
        [StringLength(maximumLength: 150)]
        [Required]
        public string TextBig { get; set; }

        [StringLength(maximumLength:150)]
        [Required]
        public string Link { get; set; }

        
        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
