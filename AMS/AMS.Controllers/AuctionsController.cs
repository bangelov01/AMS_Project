namespace AMS.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using AMS.Services.Contracts;

    using AMS.Controllers.Models;
    using static AMS.Controllers.Constants.ControllersConstants;

    public class AuctionsController(IAuctionService auctionService) : Controller
    {
        public async Task<IActionResult> All(int currentPage = 1)
        {
            var totalAuctions = await auctionService.ActiveCount();

            var maxPage = Math.Ceiling((double)totalAuctions / AuctionsPerPage);

            if (currentPage > maxPage && totalAuctions != 0)
            {
                return BadRequest();
            }

            var auctionsPerPage = await auctionService.ActivePerPage(currentPage, AuctionsPerPage);

            if (!auctionsPerPage.Any())
            {
                return View("NoResult");
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
