namespace AMS.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using AMS.Services.Contracts;
    using AMS.Web.Areas.Admin.Controllers.Base;
    using AMS.Web.Areas.Admin.Models;

    public class AuctionsController : AdminController
    {
        private readonly IAuctionService auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            this.auctionService = auctionService;
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(AuctionFormModel auction)
        {
            if (auctionService.IsAuctionCreated(auction.Number))
            {
                this.ModelState.AddModelError(nameof(auction.Number), "Auction with that number already exists!");
            }

            if (!ModelState.IsValid)
            {
                return View(auction);
            }

            auctionService.CreateAuction(auction.Number,
                auction.Description,
                auction.Start,
                auction.End,
                auction.Country,
                auction.City,
                auction.AddressText);

            return RedirectToAction("Index", "Home");
        }
    }
}
