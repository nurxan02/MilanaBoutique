using MilanaBoutique.Models;
using System.Collections.Generic;

namespace MilanaBoutique.ViewModels
{
    public class ContactVM
    {
        public Contact Contact { get; set; }
        public List<Store> Stores { get; set; }

        public Questions Question { get; set; }
    }
}
