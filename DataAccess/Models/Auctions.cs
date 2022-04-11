using System;

namespace AuctionManagement
{
    public class Auctions
    {
        public int AuctionId { get; set; }

        public string Description { get; set; }

        public DateTime AuctionDate { get; set; }

        public int AuctionItemsCount { get; set; }
    }
}
