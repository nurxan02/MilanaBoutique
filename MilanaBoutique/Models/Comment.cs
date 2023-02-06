using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }


        public short? Star { get; set; }


        public DateTime CreatedTime { get; set; }
        [Required]
        [StringLength(maximumLength: 350)]
        public string Message { get; set; }
        [Required]
        [StringLength(maximumLength: 60)]
        public string Subject { get; set; }

        public bool IsAccepted { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
