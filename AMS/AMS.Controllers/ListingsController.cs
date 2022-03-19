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

        public ListingsController(IListingService listingService,
            IValidatorService validatorService,
            IAuctionService auctionService)
        {
            this.listingService = listingService;
            this.validatorService = validatorService;
            this.auctionService = auctionService;
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

            return RedirectToAction(nameof(All), new { Id = Id });
        }

        public IActionResult All(string Id, int currentPage = 1)
        {
            var auction = auctionService.DetailsById(Id);

            var listings = listingService.ApprovedPerPage(Id, currentPage, ListingsPerPage);

            var totalListings = listingService.Count(Id);

            var maxPage = Math.Ceiling((double)totalListings / ListingsPerPage);

            if (auction == null || listings == null || currentPage > maxPage)
            {
                return BadRequest();
            }

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
                return NotFound();
            }

            var listings = listingService.Search(searchTerm.Trim());

            listings = listings.OrderByDescending(l => l.GetType().GetProperty(orderBy).GetValue(l));

            return View(new SearchListingsViewModel
            {
                Listings = listings,
                SearchTerm = searchTerm
            });
        }

        [Authorize]
        public IActionResult Details(string audtionId, string listingId)
        {
            var auction = auctionService.DetailsById(audtionId);

            var listing = listingService.Details(listingId);

            if (auction == null || listing == null)
            {
                return BadRequest();
            }

            //var model = new ListingViewModel
            //{
            //    Auction = auction,
            //    Listing = listing
            //}

            return View();
        }
    }
}
