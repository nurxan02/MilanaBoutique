using MilanaBoutique.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.ViewModels
{
    public class CountVM
    {

        public List<Category> Categories { get; set; }
        public List<ProductColor> Colors { get; set; }
    }
}
