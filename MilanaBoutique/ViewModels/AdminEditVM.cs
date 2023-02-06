using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.ViewModels
{
    public class AdminEditVM
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
    }
}
