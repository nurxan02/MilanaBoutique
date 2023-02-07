using MilanaBoutique.Models;
using System.ComponentModel.DataAnnotations;

namespace MilanaBoutique.ViewModels
{
    public class AccountVM
    {

        public AppUser AppUser { get; set; }
        public string Token { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
