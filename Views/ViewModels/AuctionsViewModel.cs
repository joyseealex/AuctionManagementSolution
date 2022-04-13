using System;
using System.ComponentModel.DataAnnotations;

namespace AuctionManagement
{
    public class AuctionsViewModel
    {
        public int AuctionId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime AuctionDate { get; set; }

        public int TotalAuctionItems { get; set; }
    }
}
