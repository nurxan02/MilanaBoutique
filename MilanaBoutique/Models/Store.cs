using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Models
{
    public class Store
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:100)]

        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]

        public string Address { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]

        public string Mail { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime StoreOpenTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime StoreCloseTime { get; set; }



        [Required]
        [StringLength(maximumLength:120)]
        public string StoreLink { get; set; }

        [Required]
        [StringLength(maximumLength: 120)]
        public string Phone { get; set; }

        public string StoreImage { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }





    }
}
