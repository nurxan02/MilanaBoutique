using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilanaBoutique.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 140)]
        public string Name { get; set; }


        [StringLength(maximumLength: 50)]
        public string Author { get; set; }

        [StringLength(maximumLength: 120)]
        public string Image { get; set; }


        [DataType(DataType.Date)]
        public DateTime CreatedTime { get; set; }
        public List<BlogComment> Comments { get; set; }


        [Required]
        public string Description { get; set; }

        [StringLength(maximumLength: 350)]
        [Required]
        public string BlackQuote { get; set; }
        //[Required]
        //public string LeaveReply { get; set; }
        public List<BlogTag> BlogTags { get; set; }


        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        [Required]
        public List<int> TagIds { get; set; }
    }
}
