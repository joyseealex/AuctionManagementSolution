using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionManagement
{
    public interface IAuctionsManager
    {
        Task<List<AuctionsViewModel>> GetAllAuctions();

        Task<AuctionsViewModel> GetAuctionById(int auctionId);

        Task<AuctionDetailsViewModel> GetAuctionDetailsById(int auctionId);

        bool AddOrEditAuction(AuctionsViewModel auctionVM);
    }
}
