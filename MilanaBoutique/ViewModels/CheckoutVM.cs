using MilanaBoutique.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilanaBoutique.ViewModels
{
    public class CheckoutVM
    {
        [Required]
        [StringLength(maximumLength: 90)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 40)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(maximumLength: 40)]
        public string Surname { get; set; }
       
       
        [Required]
        [StringLength(maximumLength: 90)]
        public string Country { get; set; }

        [Required]
        [StringLength(maximumLength: 90)]
        public string City { get; set; }

        [Required]
        [StringLength(maximumLength: 90)]
        public string Zip { get; set; }

        [Required]
        [StringLength(maximumLength: 140)]
        public string Address { get; set; }


        [Required]
        [StringLength(maximumLength: 90)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }


        [StringLength(maximumLength: 200)]

        public string OrderNote { get; set; }

        public List<BasketItem> BasketItems { get; set; }

        [StringLength(maximumLength:200)]
        public string MessagesToAdmin { get; set; }
    }
}
