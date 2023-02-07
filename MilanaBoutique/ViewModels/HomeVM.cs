using MilanaBoutique.Models;
using System.Collections.Generic;

namespace MilanaBoutique.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public Category Category { get; set; }
        public List<Brand> Brands { get; set; }
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public List<Blog> Blogs { get; set; }

        public Settings Settings { get; set; }

        public List<ProductSizeColor> ProductSizeColors { get; set; }
        public ProductSizeColor ProductSizeColor { get; set; }


        public List<ProductColor> ProductColors { get; set; }
        public List<ProductColor> ProductColors2 { get; set; }

        public List<Comment> Comments { get; set; }

        public Subscriber Subscriber { get; set; }
    }
}
