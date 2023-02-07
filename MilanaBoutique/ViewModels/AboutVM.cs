using MilanaBoutique.Models;
using System.Collections.Generic;

namespace MilanaBoutique.ViewModels
{
    public class AboutVM
    {
        public List<Questions> Questions { get; set; }
        public AboutUs AboutUs { get; set; }

        public List<Member> Members { get; set; }
        public List<Brand> Brands { get; set; }
    }
}
