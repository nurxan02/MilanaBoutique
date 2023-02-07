using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilanaBoutique.Models
{
    public class Contact
    {
        public int Id { get; set; }

        public string BannerImage { get; set; }

        [NotMapped]
        public IFormFile BannerFile { get; set; }

        [Required]
        [StringLength(maximumLength:100)]
        public string BannerTitle { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string BannerDesc { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string ContactInformation { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string OfficeLocation { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string LocationIcon { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Phone { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string PhoneIcon { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Mail { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string MailIcon { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public string OpenTime { get; set; }


        [Required]
        [DataType(DataType.Time)]
        public string CloseTime { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string OclockIcon { get; set; }
    }
}
