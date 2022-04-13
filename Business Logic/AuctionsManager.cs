using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<AuctionsViewModel> GetAuctionById(int auctionId)
        {
            IEnumerable<AuctionsViewModel> auctionsList = await GetAllAuctions().ConfigureAwait(false);
            return auctionsList.Where(x => x.AuctionId == auctionId).FirstOrDefault();
        }

        public async Task<List<AuctionsViewModel>> GetAllAuctions()
        {
            var auctions = new List<AuctionsViewModel>();

            try
            {
                var auctionsList = await AuctionDetailsRetreiver.GetAllAuctions().ConfigureAwait(false);

                foreach (var item in auctionsList)
                {
                    auctions.Add(new AuctionsViewModel
                    {
                        AuctionId = Convert.ToInt32(item.AuctionId),
                        Description = item.Description,
                        AuctionDate = item.AuctionDate,
                        TotalAuctionItems = Convert.ToInt32(item.TotalAuctionItems)
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
            var auctionItemsVM = new AuctionDetailsViewModel();

            try
            {
                var auctionItemsList = await AuctionDetailsRetreiver.GetAuctionDetailsByIdAsync(auctionId).ConfigureAwait(false);
                var auctionItems = new List<AuctionItems>();

                if (auctionItemsList.ToList().Count > 0)
                {
                    foreach (var item in auctionItemsList)
                    {
                        auctionItemsVM.Auction = new Auctions
                        {
                            AuctionDate = item.AuctionDate,
                            AuctionId = item.AuctionId,
                            Description = item.AuctionDescription
                        };

                        auctionItems.Add(new AuctionItems
                        {
                            ItemId = item.ItemId,
                            ItemDescription = item.ItemDescription,
                            StartPrice = item.StartPrice
                        });
                    }

                    auctionItemsVM.AuctionItems = auctionItems;
                }
                else
                {
                    var auctionDetails = await GetAuctionById(auctionId).ConfigureAwait(false);
                    auctionItemsVM.Auction = new Auctions
                    {
                        AuctionDate = auctionDetails.AuctionDate,
                        AuctionId = auctionDetails.AuctionId,
                        Description = auctionDetails.Description
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return auctionItemsVM;
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
    }
}
