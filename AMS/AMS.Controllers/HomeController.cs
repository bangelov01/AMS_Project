namespace AMS.Controllers
{
    using AMS.Controllers.Models;
    using AMS.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        private readonly IListingService listingService;
        private readonly IAuctionService auctionService;
        private readonly IUserService userService;

        public HomeController(IListingService listingService,
            IAuctionService auctionService,
            IUserService userService)
        {
            this.listingService = listingService;
            this.auctionService = auctionService;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            return View(new HomeViewModel
            {
                Auctions = await auctionService.Total(),
                Listings = await listingService.Total(),
                Users = await userService.Total(),
            });
        }
    }
}