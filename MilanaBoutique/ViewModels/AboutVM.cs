using MilanaBoutique.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
