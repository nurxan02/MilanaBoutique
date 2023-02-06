using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Models
{
    public class Gender
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:50)]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
