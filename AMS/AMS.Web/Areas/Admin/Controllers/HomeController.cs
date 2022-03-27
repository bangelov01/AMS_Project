namespace AMS.Web.Areas.Admin.Controllers
{
    using AMS.Services.Contracts;
    using AMS.Web.Areas.Admin.Controllers.Base;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : AdminController
    {
        private readonly IListingService listingService;

        public HomeController(IListingService listingService)
        {
            this.listingService = listingService;
        }

        public async Task<IActionResult> Index()
        {
            var listings = await listingService.NotApproved();

            return View(listings);
        }

        public async Task<IActionResult> Approve(string Id)
        {
            bool isApproved = await listingService.Approve(Id);

            if (!isApproved)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<IActionResult> Delete(string Id)
        {
            bool isDeleted = await listingService.Delete(Id);

            if (!isDeleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
