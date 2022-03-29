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

        public HomeController(IListingService listingService)
        {
            this.listingService = listingService;
        }

        public async Task<IActionResult> Index()
        {
            var listings = await listingService.Preview();

            return View(listings);
        }
    }
}