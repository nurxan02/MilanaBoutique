using System.ComponentModel.DataAnnotations;

namespace MilanaBoutique.ViewModels
{
    public class LoginVM
    {
        [StringLength(maximumLength: 30)]
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
