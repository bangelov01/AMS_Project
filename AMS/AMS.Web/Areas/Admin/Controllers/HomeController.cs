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
            => View(await listingService.NotApproved());

        public async Task<IActionResult> Approve(string Id)
        {
            if (string.IsNullOrEmpty(Id) || !await listingService.Approve(Id))
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<IActionResult> Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id) || !await listingService.Delete(Id))
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
