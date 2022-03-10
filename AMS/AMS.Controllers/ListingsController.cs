namespace AMS.Controllers
{
    using AMS.Controllers.Models;
    using AMS.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ListingsController : Controller
    {
        private readonly IListingService listingService;

        public ListingsController(IListingService listingService)
        {
            this.listingService = listingService;
        }

        [Authorize]
        public IActionResult Create(int vehicleTypeId,
                                    int makeId,
                                    int modelId)
        {
            var listing = new ListingFormModel();
            listing.Conditions = listingService.AllConditions();
            listing.VehicleTypes = listingService.AllTypes();

            if (vehicleTypeId != 0)
            {
                listing.Makes = listingService.MakesByType(vehicleTypeId);
            }
            if (makeId != 0)
            {
                listing.Models = listingService.ModelsByMake(makeId);
            }

            return View(listing);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(string Id, ListingFormModel listing)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create), new
                {
                    vehicleTypeId = listing.VehicleTypeId,
                    makeId = listing.MakeId,
                    modelId = listing.ModelId,
                });
            }

            return View("Index", "Home");
        }
    }
}
