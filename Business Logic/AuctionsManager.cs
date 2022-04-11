using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionManagement
{
    public class AuctionsManager : IAuctionsManager
    {
        readonly ILogger<AuctionsManager> _logger;
        public IAuctionDetailsRetreiver AuctionDetailsRetreiver { get; set; }

        public AuctionsManager(ILogger<AuctionsManager> logger, IAuctionDetailsRetreiver auctionDetailsRetreiver)
        {
            _logger = logger;
            AuctionDetailsRetreiver = auctionDetailsRetreiver;
        }

        public List<AuctionsViewModel> GetAllAuctions()
        {
            var auctions = new List<AuctionsViewModel>();

            try
            {
                var auctionsDataTable = AuctionDetailsRetreiver.GetAllAuctions();

                for (int i = 0; i < auctionsDataTable.Rows.Count; i++)
                {
                    auctions.Add(new AuctionsViewModel
                    {
                        AuctionId = Convert.ToInt32(auctionsDataTable.Rows[i]["AuctionId"]),
                        Description = auctionsDataTable.Rows[i]["Description"].ToString(),
                        AuctionItemCount = Convert.ToInt32(auctionsDataTable.Rows[i]["AuctionItemCount"])
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return auctions;
        }

        public async Task<AuctionDetailsViewModel> GetAuctionDetailsById(int auctionId)
        {
            var auctionItems = new AuctionDetailsViewModel();

            try
            {
                auctionItems = await AuctionDetailsRetreiver.GetAuctionDetailsByIdAsync(auctionId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return auctionItems;
        }

        public bool AddOrEditAuction(AuctionsViewModel auctionVM)
        {
            try
            {
                AuctionDetailsRetreiver.AddOrEditAuction(auctionVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }

            return true;
        }

        public bool AddOrEditAuction(AuctionItemViewModel auctionItemVM)
        {
            try
            {
                AuctionDetailsRetreiver.AddOrEditAuctionItem(auctionItemVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }

            return true;
        }
    }
}
