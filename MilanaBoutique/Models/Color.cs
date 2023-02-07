using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilanaBoutique.Models
{
    public class Color
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength:25)]
        public string Name { get; set; }
        //public List<Product> Products { get; set; }
        public List<ProductColor> ProductColors { get; set; }
        public List<ProductSizeColor> ProductSizeColors { get; set; }

    }
}
