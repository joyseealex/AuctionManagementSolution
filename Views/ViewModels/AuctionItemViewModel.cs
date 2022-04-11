using System;
using System.ComponentModel.DataAnnotations;

namespace AuctionManagement
{
    public class AuctionItemViewModel
    {
        public int ItemId { get; set; }

        [Required]
        public int AuctionId { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Start Price can't be less than zero")]
        public decimal StartPrice { get; set; }

    }
}
