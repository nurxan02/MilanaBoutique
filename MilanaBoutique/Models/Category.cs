using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength:50)]
        public string Name { get; set; }
        public string Image { get; set; }

        [NotMapped]

        public IFormFile ImageFile { get; set; }

        public string SizeGuideImage { get; set; }

        [NotMapped]

        public IFormFile SizeGuideFile { get; set; }

        public int? GenderId { get; set; }

        public Gender Gender { get; set; }

        public List<Product> Products { get; set; }

        public List<SubCategory> SubCategories { get; set; }
    }
}
