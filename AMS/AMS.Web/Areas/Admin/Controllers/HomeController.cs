namespace AMS.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using AMS.Services.Contracts;

    using AMS.Web.Areas.Admin.Controllers.Base;

    public class HomeController(IListingService listingService) : AdminController
    {
        public async Task<IActionResult> Index() 
            => View(await listingService.NotApproved());

        public async Task<IActionResult> Approve(string Id)
        {
            if (string.IsNullOrEmpty(Id) || !await listingService.Approve(Id))
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id) || !await listingService.Delete(Id))
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
