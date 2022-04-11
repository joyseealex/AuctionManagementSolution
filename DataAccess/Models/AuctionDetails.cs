using System;

namespace AuctionManagement
{
    public class AuctionDetails
    {
        public int ItemId { get; set; }

        public int AuctionId { get; set; }

        public string ItemDescription { get; set; }

        public string AuctionDescription { get; set; }

        public DateTime AuctionDate { get; set; }

        public decimal StartPrice { get; set; }
    }
}
