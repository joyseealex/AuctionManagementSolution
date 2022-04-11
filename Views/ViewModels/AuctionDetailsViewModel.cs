using System.Collections.Generic;

namespace AuctionManagement
{
    public class AuctionDetailsViewModel
    {
        public Auctions Auction { get; set; }

        public List<AuctionItems> AuctionItems { get; set; }
    }
}
