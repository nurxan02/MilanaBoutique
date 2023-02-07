using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MilanaBoutique.Models
{
    public class AppUser:IdentityUser
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsBlocked { get; set; }


        public string Country { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public List<BasketItem> BasketItems { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Order> Orders { get; set; }

        public List<WishlistItem> WishlistItems { get; set; }
    }
}
