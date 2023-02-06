using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Models
{
    public class Size
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:15)]
        public string Name { get; set; }

        //public List<Product> Products { get; set; }
        public List<ProductSizeColor> ProductSizeColors { get; set; }

    }
}
