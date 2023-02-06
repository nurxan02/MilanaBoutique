using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
