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

        public IActionResult Index()
        {
            var listings = listingService.NotApproved();

            return View(listings);
        }

        public IActionResult Approve(string Id)
        {
            bool isApproved = listingService.Approve(Id);

            if (!isApproved)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult Delete(string Id)
        {
            bool isDeleted = listingService.Delete(Id);

            if (!isDeleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
