using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Models
{
    public class Subscriber
    {
        public int Id { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
    }
}
