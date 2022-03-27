namespace AMS.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using AMS.Services.Contracts;
    using static AMS.Services.Infrastructure.Extensions.ClaimsPrincipleExtensions;

    using AMS.Controllers.Models;
    using static AMS.Controllers.Constants.ControllersConstants;

    public class ListingsController : Controller
    {
        private readonly IListingService listingService;
        private readonly IValidatorService validatorService;
        private readonly IAuctionService auctionService;
        private readonly IBidService bidService;

        public ListingsController(IListingService listingService,
            IValidatorService validatorService,
            IAuctionService auctionService,
            IBidService bidService)
        {
            this.listingService = listingService;
            this.validatorService = validatorService;
            this.auctionService = auctionService;
            this.bidService = bidService;
        }

        [Authorize]
        public async Task<IActionResult> Create(string Id)
        {
            if (!validatorService.IsAuctionValid(Id))
            {
                return BadRequest();
            }

            return View(new ListingsFormModel
            {
                Conditions = await listingService.Conditions(),
                Types = await listingService.Types(),
                Makes = await listingService.Makes(),
                Models = await listingService.Models()
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(string Id, ListingsFormModel listing)
        {
            if (!validatorService.AreListingParamsValid(listing.ConditionId, listing.ModelId))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                listing.Conditions = await listingService.Conditions();
                listing.Types = await listingService.Types();
                listing.Makes = await listingService.Makes();
                listing.Models = await listingService.Models();

                return View(listing);
            }

            await listingService.Create(listing.Year,
                listing.Price,
                listing.Description,
                listing.ImageUrl,
                listing.ConditionId,
                Id,
                listing.ModelId,
                this.User.Id());

            return RedirectToAction(nameof(All), new { Id = Id });
        }

        public async Task<IActionResult> All(string Id, int currentPage = 1)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return BadRequest();
            }

            var auction = await auctionService.DetailsById(Id);

            if (auction == null)
            {
                return View("NoResult");
            }

            var totalListings = await listingService.Count(Id);

            var maxPage = Math.Ceiling((double)totalListings / ListingsPerPage);

            if (currentPage > maxPage && totalListings != 0)
            {
                return BadRequest();
            }

            var listings = await listingService.ApprovedPerPage(Id, currentPage, ListingsPerPage);

            var model = new AllListingsViewModel
            {
                Listings = listings,
                Auction = auction,
                Pagination = new PaginationViewModel
                {
                    Id = Id,
                    CurrentPage = currentPage,
                    ItemsCount = listings.Count(),
                    MaxPage = maxPage,
                    ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString()
                }
            };

            return View(model);
        }

        public async Task<IActionResult> Find(string searchTerm, string orderBy = DefaultOrderParam)
        {

            if (string.IsNullOrEmpty(searchTerm) || !validatorService.IsOrderParamValid(orderBy))
            {
                return BadRequest();
            }

            var listings = await listingService.Search(searchTerm.Trim());

            if (!listings.Any())
            {
                return View("NoResult");
            }

            listings = listings.OrderByDescending(l => l.GetType().GetProperty(orderBy).GetValue(l));

            return View(new SearchListingsViewModel
            {
                Listings = listings,
                SearchTerm = searchTerm
            });
        }

        [Authorize]
        public async Task<IActionResult> Details(string auctionId, string listingId)
        {
            if (string.IsNullOrEmpty(auctionId) || string.IsNullOrEmpty(listingId))
            {
                return BadRequest();
            }

            var auction = await auctionService.DetailsById(auctionId);

            if (auction == null)
            {
                return View("NoResult");
            }

            return View(new ListingViewModel
            {
                Auction = auction,
                Listing = await listingService.Details(listingId, this.User.Id()),
                Bid = bidService.HighestForListing(listingId)
            });
        }
    }
}
