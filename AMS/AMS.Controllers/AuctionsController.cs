namespace AMS.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using AMS.Services.Contracts;

    using AMS.Controllers.Models;
    using static AMS.Controllers.Constants.ControllersConstants;

    public class AuctionsController : Controller
    {
        private readonly IAuctionService auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            this.auctionService = auctionService;
        }

        public async Task<IActionResult> All(int currentPage = 1)
        {
            var auctionsPerPage = await auctionService.ActivePerPage(currentPage, AuctionsPerPage);

            if (!auctionsPerPage.Any())
            {
                return View("NoResult");
            }

            var totalAuctions = await auctionService.ActiveCount();

            var maxPage = Math.Ceiling((double)totalAuctions / AuctionsPerPage);

            if (currentPage > maxPage)
            {
                return BadRequest();
            }

            var auctions = new AllAuctionsViewModel
            {
                Auctions = auctionsPerPage,
                Total = totalAuctions,
                Pagination = new PaginationViewModel
                {
                    CurrentPage = currentPage,
                    ItemsCount = auctionsPerPage.Count(),
                    MaxPage = maxPage,
                    ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString(),
                }
            };

            return View(auctions);
        }
    }
}
