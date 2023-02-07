namespace MilanaBoutique.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        public string Image { get; set; }
        public int ProductSizeColorId { get; set; }
        public ProductSizeColor ProductSizeColor { get; set; }

    }
}
