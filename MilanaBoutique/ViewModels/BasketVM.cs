using System.Collections.Generic;

namespace MilanaBoutique.ViewModels
{
    public class BasketVM
    {
        public List<BasketItemVM> BasketItems { get; set; }
        public double? TotalPrice { get; set; }
        public int Count { get; set; }
    }
}
