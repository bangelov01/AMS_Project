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
        public IActionResult Create(string Id)
        {
            if (!validatorService.IsAuctionValid(Id))
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
            if (!validatorService.AreListingParamsValid(listing.ConditionId, listing.ModelId))
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

            return RedirectToAction(nameof(All), new { Id = Id });
        }

        public IActionResult All(string Id, int currentPage = 1)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return BadRequest();
            }

            var auction = auctionService.DetailsById(Id);

            if (auction == null)
            {
                return View("NoResult");
            }

            var totalListings = listingService.Count(Id);

            var maxPage = Math.Ceiling((double)totalListings / ListingsPerPage);

            if (currentPage > maxPage && totalListings != 0)
            {
                return BadRequest();
            }

            var listings = listingService.ApprovedPerPage(Id, currentPage, ListingsPerPage);

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

        public IActionResult Find(string searchTerm, string orderBy = DefaultOrderParam)
        {

            if (string.IsNullOrEmpty(searchTerm) || !validatorService.IsOrderParamValid(orderBy))
            {
                return BadRequest();
            }

            var listings = listingService.Search(searchTerm.Trim());

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
        public IActionResult Details(string auctionId, string listingId)
        {
            if (string.IsNullOrEmpty(auctionId) || string.IsNullOrEmpty(listingId))
            {
                return BadRequest();
            }

            var auction = auctionService.DetailsById(auctionId);

            if (auction == null)
            {
                return View("NoResult");
            }

            return View(new ListingViewModel
            {
                Auction = auction,
                Listing = listingService.Details(listingId, this.User.Id()),
                Bid = bidService.HighestForListing(listingId)
            });
        }
    }
}
