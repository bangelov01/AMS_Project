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
                Pagination = new PaginationViewModel
                {
                    CurrentPage = currentPage,
                    TotalItems = auctionService.AllActiveCount(),
                    ItemsCount = query.Count(),
                    ItemsPerPage = AuctionsPerPage,
                    ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString(),
                }
            };

            return View(auctions);
        }
    }
}
