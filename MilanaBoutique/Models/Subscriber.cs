using System.ComponentModel.DataAnnotations;

namespace MilanaBoutique.Models
{
    public class Subscriber
    {
        public int Id { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
    }
}
