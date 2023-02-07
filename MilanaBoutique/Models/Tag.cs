using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilanaBoutique.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:50)]
        public string Name { get; set; }

        public List<BlogTag> BlogTags { get; set; }
    }
}
