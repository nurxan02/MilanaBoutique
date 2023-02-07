using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilanaBoutique.Models
{
    public class Order
    {

        public int Id { get; set; }


        [Required]

        public string Firstname { get; set; }
        [Required]

        public string Lastname { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]

        public string Zip { get; set; }
        [Required]
        public double? TotalPrice { get; set; }
        [Required]
        public DateTime Date { get; set; }
        //public bool? Status { get; set; }
        [StringLength(maximumLength: 200)]

        public string MessageToUser { get; set; }

        [StringLength(maximumLength:200)]
        public string MessageToAdmin { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public Status Status { get; set; }
        public int StatusId { get; set; }
    }
}
