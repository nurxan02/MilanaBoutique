using System;
using System.ComponentModel.DataAnnotations;

namespace MilanaBoutique.Models
{
    public class BlogComment
    {
        public int Id { get; set; }
        public Blog Blog { get; set; }
        public int BlogId { get; set; }
        public DateTime CreatedTime { get; set; }
        [Required]
        [StringLength(maximumLength: 350)]
        public string Message { get; set; }

        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }

        public bool IsAccepted { get; set; }


    }
}
