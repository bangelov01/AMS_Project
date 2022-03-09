namespace AMS.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using AMS.Controllers.Models;
    using AMS.Services.Contracts;

    using static AMS.Controllers.Constants.ControllersConstants;

    public class AuctionsController : Controller
    {
        private readonly IAuctionService auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            this.auctionService = auctionService;
        }

        public IActionResult All(int currentPage = 1)
        {
            var query = auctionService.AllActivePerPage(currentPage, AuctionsPerPage);

            var auctions = new AllAuctionsViewModel
            {
                Auctions = query,
                CurrentPage = currentPage,
                TotalAuctions = auctionService.AllActiveCount()
            };

            return View(auctions);
        }
    }
}
