using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AuctionManagement.Controllers
{
    public class AuctionsController : Controller
    {
        public ILogger<AuctionsController> Log { get; set; }
        public IAuctionsManager AuctionsManager { get; set; }

        public AuctionsController(ILogger<AuctionsController> log, IAuctionsManager auctionsManager)
        {
            Log = log ?? throw new ArgumentNullException(nameof(log));
            AuctionsManager = auctionsManager ?? throw new ArgumentNullException(nameof(auctionsManager));
        }

        public async Task<IActionResult> Index()
        {
            var auctionsList = await AuctionsManager.GetAllAuctions().ConfigureAwait(false);
            return View(auctionsList);
        }
        
        public async Task<IActionResult> AddOrEditAuctions(int? id)
        {
            var auctionDetails = new AuctionsViewModel();

            if (id != null)
                auctionDetails = await AuctionsManager.GetAuctionById((int)id).ConfigureAwait(false);

            return View(auctionDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEditAuctions(int id, [Bind("AuctionId,Description,AuctionDate")] AuctionsViewModel auctionVM)
        {
            if (ModelState.IsValid)
            {
                var isAuctionCreatedOrEdited = AuctionsManager.AddOrEditAuction(auctionVM);

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(ViewAuctionItems));
        }

        public async Task<IActionResult> ViewAuctionItems(int? id)
        {
            var auctionItems = new AuctionDetailsViewModel();

            if (id != null)
                auctionItems = await AuctionsManager.GetAuctionDetailsById((int)id).ConfigureAwait(false);

            return View(auctionItems);
        }
    }
}
