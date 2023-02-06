using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Models
{
    public class AboutUs
    {
        public int Id { get; set; }

        public string Banner { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [Required]
        [StringLength(maximumLength:50)]
        public string Title { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string SubTitle { get; set; }

        [Required]
        [StringLength(maximumLength: 250)]
        public string Vision { get; set; }

        [Required]
        [StringLength(maximumLength: 250)]
        public string Mission { get; set; }

        [Required]
        [StringLength(maximumLength: 150)]
        public string NewsLink { get; set; }
    }
}
