using MilanaBoutique.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.ViewModels
{
    public class ShopVM
    {
        public Category Category { get; set; }
        public List<Brand> Brands { get; set; }
        public Product Product { get; set; }
        public List<Product> Products { get; set; }

        public List<ProductSizeColor> ProductSizeColors { get; set; }
        public ProductSizeColor ProductSizeColor { get; set; }


        public List<ProductColor>  ProductColors { get; set; }
        public List<ProductColor> ProductColors2 { get; set; }

        public List<Comment> Comments { get; set; }
        public Comment Comment { get; set; }

        public SubCategory SubCategory { get; set; }
    }
}
