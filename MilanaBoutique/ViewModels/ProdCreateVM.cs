using MilanaBoutique.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.ViewModels
{
    public class ProdCreateVM
    {
        public Product Product { get; set; }
        public ProductSizeColor ProductSizeColor { get; set; }
    }
}
