using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilanaBoutique.Models
{
    public class Product
    {
        public int Id { get; set; }

        public short? AvgStar { get; set; }

        public int TotalSold { get; set; }

        [Required]
        [StringLength(maximumLength: 120)]
        public string Name { get; set; }


        public bool DealOfTheDay { get; set; }


        [Required]

        public double Price { get; set; }

        [StringLength(maximumLength: 160)]

        public string Description { get; set; }

        public Gender Gender { get; set; }

        public int GenderId { get; set; }
        public short? Rating { get; set; } 

        public double? Discount { get; set; }

        [Required]
        [StringLength(maximumLength: 120)]
        public string Subtitle { get; set; }



        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }
        

        public int BrandId { get; set; }
        public Brand Brand { get; set; }


        public List<ProductSizeColor> ProductSizeColors { get; set; }

        public List<ProductColor> ProductColors { get; set; }

        public List<Comment> Comments { get; set; }

        //[NotMapped]
        //[Required]
        //public int ColorId { get; set; }

        //[NotMapped]
        //[Required]
        //public int SizeId { get; set; }

        //[NotMapped]
        //[Required]
        //public int Stock { get; set; }

        //[NotMapped]
        //public int MyProperty { get; set; }

        //[NotMapped]
        //public int MyProperty { get; set; }

        //public int SizeId { get; set; }

        //public Size Size { get; set; }

        //public int ColorId { get; set; }
        //public Color Color { get; set; }
    }
}
