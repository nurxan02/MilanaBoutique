using MilanaBoutique.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilanaBoutique.ViewModels
{
    public class UserEditVM
    {
        [StringLength(maximumLength: 30)]
        [Required]
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]

        public string Email { get; set; }
        [StringLength(maximumLength: 40)]
        [Required]

        public string Firstname { get; set; }
        [StringLength(maximumLength: 40)]
        [Required]

        public string Surname { get; set; }


        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        //public bool TermsAndConditions { get; set; }
        [StringLength(maximumLength: 90)]
        [Required]

        public string Country { get; set; }
        [StringLength(maximumLength: 90)]
        [Required]

        public string City { get; set; }
        [StringLength(maximumLength: 90)]
        [Required]

        public string Zip { get; set; }
        [StringLength(maximumLength: 90)]
        [Required]

        public string Address { get; set; }
        [StringLength(maximumLength: 90)]
        [DataType(DataType.PhoneNumber)]
        [Required]

        public string Phone { get; set; }

        public List<Order> Order { get; set; }

        


    }
}
