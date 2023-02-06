using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Models
{
    public class Settings
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength:50)]
        public string Phone { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string SearchIcon { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string WishlistIcon { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string BasketIcon { get; set; }

        
        public string HeaderLogoImage { get; set; }

        [NotMapped]
        public IFormFile HeaderLogoImageFile { get; set; }




        public string FooterLogoImage { get; set; }

        [NotMapped]
        public IFormFile FooterLogoImageFile { get; set; }


        [Required]
        [StringLength(maximumLength: 50)]
        public string FbIcon { get; set; }

        [Required]
        [StringLength(maximumLength: 160)]
        public string FbLink { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string InstaIcon { get; set; }

        [Required]
        [StringLength(maximumLength: 160)]
        public string InstaLink { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string TwitIcon { get; set; }

        [Required]
        [StringLength(maximumLength: 160)]
        public string TwitLink { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string YoutubeIcon { get; set; }
        [Required]
        [StringLength(maximumLength: 160)]
        public string YtLink { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string PinterestIcon { get; set; }

        [Required]
        [StringLength(maximumLength: 160)]
        public string PinterestLink { get; set; }

        [Required]
        [StringLength(maximumLength: 160)]
        public string FooterDesc { get; set; }

        [Required]
        [StringLength(maximumLength: 160)]
        public string Copyright { get; set; }


        [Required]
        [StringLength(maximumLength: 50)]
        public string HomePageRefundIcon { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string HomePageDeliveryIcon { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string HomePagePaymentIcon { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string HomePageSupportIcon { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string PhoneIcon { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string UserIcon { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string LittleWishlist { get; set; }

    }
}
