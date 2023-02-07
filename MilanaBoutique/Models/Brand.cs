using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilanaBoutique.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:65)]
        public string Name { get; set; }
        public List<Product> Products { get; set; }

        public string Image { set; get; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public int? Totalsold { get; set; }
    }
}
