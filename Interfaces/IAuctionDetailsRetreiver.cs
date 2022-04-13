using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionManagement
{
    public interface IAuctionDetailsRetreiver
    {
        Task<IEnumerable<Auctions>> GetAllAuctions();

        Task<IEnumerable<AuctionDetails>> GetAuctionDetailsByIdAsync(int auctionId);

        void AddOrEditAuction(AuctionsViewModel auctionVM);

        void AddOrEditAuctionItem(AuctionItemViewModel auctionItemsVM);
    }
}
