namespace AMS.Controllers
{
    using AMS.Controllers.Models;
    using AMS.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static AMS.Services.Constants.ServicesConstants;

    public class AuctionsController : Controller
    {
        private readonly IAuctionService auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            this.auctionService = auctionService;
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = AdministratorRoleName)]
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
