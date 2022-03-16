namespace AMS.Controllers
{
    using AMS.Controllers.Models;
    using AMS.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static AMS.Services.Infrastructure.Extensions.ClaimsPrincipleExtensions;
    using static AMS.Controllers.Constants.ControllersConstants;

    public class ListingsController : Controller
    {
        private readonly IListingService listingService;
        private readonly IValidatorService validatorService;
        private readonly IUserService userService;

        public ListingsController(IListingService listingService,
            IValidatorService validatorService,
            IUserService userService)
        {
            this.listingService = listingService;
            this.validatorService = validatorService;
            this.userService = userService;
        }

        [Authorize]
        public IActionResult Create(string auctionId)
        {
            if (!validatorService.IsAuctionValid(auctionId))
            {
                return BadRequest();
            }

            return View(new ListingsFormModel
            {
                Conditions = listingService.Conditions(),
                Types = listingService.Types(),
                Makes = listingService.Makes(),
                Models = listingService.Models()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(string Id, ListingsFormModel listing)
        {
            if (!validatorService.IsListingValid(listing.TypeId, listing.ConditionId, listing.MakeId, listing.ModelId))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                listing.Conditions = listingService.Conditions();
                listing.Types = listingService.Types();
                listing.Makes = listingService.Makes();
                listing.Models = listingService.Models();

                return View(listing);
            }

            listingService.Create(listing.Year,
                listing.Price,
                listing.Description,
                listing.ImageUrl,
                listing.ConditionId,
                Id,
                listing.ModelId,
                this.User.Id());

            return RedirectToAction(nameof(All), new { auctionId = Id });
        }

        public IActionResult All(string Id, int currentPage = 1)
        {
            var auctionListings = listingService.DetailsListingsPerPage(Id, currentPage, ListingsPerPage);

            var totalListings = listingService.Count(Id);

            if (auctionListings == null || currentPage > Math.Ceiling((double)totalListings / ListingsPerPage))
            {
                return BadRequest();
            }

            var listings = new AllListingsViewModel
            {
                Listings = auctionListings.Listings,
                Id = Id,
                TotalListings = totalListings,
                Number = auctionListings.Number,
                Start = auctionListings.Start,
                End = auctionListings.End,
                City = auctionListings.City,
                CurrentPage = currentPage
            };

            return View(listings);
        }
    }
}
