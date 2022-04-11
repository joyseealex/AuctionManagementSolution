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

        // GET: Auctions
        public IActionResult Index()
        {
            var auctionsList = AuctionsManager.GetAllAuctions();
            return View(auctionsList);
        }

        // GET: Auctions/AddOrEditAuctions/
        public async Task<IActionResult> AddOrEditAuctions(int? id)
        {
            var auctionsList = new AuctionDetailsViewModel();

            if (id != null)
                auctionsList = await AuctionsManager.GetAuctionDetailsById((int)id).ConfigureAwait(false);

            return View(auctionsList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEditAuctions(int id, [Bind("AuctionId,Description,AuctionDate")] Auctions auctionsModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
