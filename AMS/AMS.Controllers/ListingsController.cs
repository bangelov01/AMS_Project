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
        private readonly IAuctionService auctionService;
        private readonly IUserService userService;

        public ListingsController(IListingService listingService,
            IValidatorService validatorService,
            IAuctionService auctionService,
            IUserService userService)
        {
            this.listingService = listingService;
            this.validatorService = validatorService;
            this.auctionService = auctionService;
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
        public IActionResult Create(string auctionId, ListingsFormModel listing)
        {
            if (!validatorService.IsListingValid(listing.TypeId, listing.ConditionId, listing.MakeId, listing.ModelId))
            {
                ModelState.AddModelError(nameof(listing.ConditionId), "One or more vehicle properties are invalid!");
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
                auctionId,
                listing.ModelId,
                this.User.Id());

            return RedirectToAction(nameof(All), new { auctionId = auctionId });
        }

        public IActionResult All(string auctionId, int currentPage = 1)
        {
            if (!validatorService.IsAuctionValid(auctionId))
            {
                return BadRequest();
            }

            var currentAuction = auctionService.DetailsListingsPerPage(auctionId, currentPage, ListingsPerPage);

            var listings = new AllListingsViewModel
            {
                Listings = currentAuction.Listings,
                Id = auctionId,
                TotalListings = listingService.Count(),
                Number = currentAuction.Number,
                Start = currentAuction.Start,
                End = currentAuction.End,
                City = currentAuction.City,
                CurrentPage = currentPage
            };

            return View(listings);
        }
    }
}
