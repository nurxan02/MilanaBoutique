using MilanaBoutique.Models;

namespace MilanaBoutique.ViewModels
{
    public class BasketItemVM
    {
        public ProductSizeColor ProductSizeColor { get; set; }
        public double? Price { get; set; }
        public int Count { get; set; }
    }
}
