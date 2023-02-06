using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }

        public ProductColor ProductColor { get; set; }

        public int ProductColorId { get; set; }

        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
    }
}
