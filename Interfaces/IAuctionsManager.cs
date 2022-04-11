using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionManagement
{
    public interface IAuctionsManager
    {
        List<AuctionsViewModel> GetAllAuctions();

        Task<AuctionDetailsViewModel> GetAuctionDetailsById(int auctionId);

        bool AddOrEditAuction(AuctionsViewModel auctionVM);

        bool AddOrEditAuction(AuctionItemViewModel auctionItemVM);
    }
}
