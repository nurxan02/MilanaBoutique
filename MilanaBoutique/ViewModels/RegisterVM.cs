using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [StringLength(maximumLength: 30)]
        public string Username { get; set; }
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
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public bool TermsAndConditions { get; set; }
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
    }
}
