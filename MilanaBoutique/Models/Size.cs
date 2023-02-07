using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilanaBoutique.Models
{
    public class Size
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:15)]
        public string Name { get; set; }

        public List<ProductSizeColor> ProductSizeColors { get; set; }

    }
}
