using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilanaBoutique.Models
{
    public class Status
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Order> Orders { get; set; }
    }
}
