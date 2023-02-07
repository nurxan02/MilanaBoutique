using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilanaBoutique.Models
{
    public class Member
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:55)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(maximumLength: 55)]
        public string Lastname { get; set; }
        [Required]
        [StringLength(maximumLength: 105)]
        public string Info { get; set; }

        [Required]
        [StringLength(maximumLength: 55)]
        public string Speciality { get; set; }

        public List<SocialMedia> SocialMedias { get; set; }

        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
