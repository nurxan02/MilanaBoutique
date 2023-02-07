using MilanaBoutique.Models;
using System.Collections.Generic;

namespace MilanaBoutique.ViewModels
{
    public class BlogVM
    {
        public List<Blog> Blogs { get; set; }
        public Blog Blog { get; set; }
        public List<Tag> Tags { get; set; }
      
    }
}
