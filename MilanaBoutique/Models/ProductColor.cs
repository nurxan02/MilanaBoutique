﻿using System.Collections.Generic;

namespace MilanaBoutique.Models
{
    public class ProductColor
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ColorId { get; set; }

        public Color Color { get; set; }

        public List<WishlistItem> WishlistItems { get; set; }
    }
}