using System.Data;
using System.Threading.Tasks;

namespace AuctionManagement
{
    public interface IAuctionDetailsRetreiver
    {
        DataTable GetAllAuctions();

        Task<AuctionDetailsViewModel> GetAuctionDetailsByIdAsync(int auctionId);

        void AddOrEditAuction(AuctionsViewModel auctionsVM);

        void AddOrEditAuctionItem(AuctionItemViewModel auctionItemsVM);
    }
}
