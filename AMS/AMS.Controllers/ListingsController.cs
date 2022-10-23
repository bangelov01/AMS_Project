namespace AMS.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using AMS.Services.Contracts;
    using static AMS.Services.Infrastructure.Extensions.ClaimsPrincipleExtensions;

    using AMS.Controllers.Models;
    using AMS.Controllers.Models.Listings;
    using static AMS.Controllers.Constants.ControllersConstants;

    [Authorize]
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

        public async Task<IActionResult> GetMakes(int typeId)
        {
            var makes = await listingService.Makes(typeId);

            if (!makes.Any())
            {
                return BadRequest();
            }

            return Ok(makes);
        }

        public async Task<IActionResult> GetModels(int typeId, int makeId)
        {
            var models = await listingService.Models(typeId, makeId);

            if (!models.Any())
            {
                return BadRequest();
            }

            return Ok(models);
        }

        public async Task<IActionResult> Create(string Id)
        {
            if (!await validatorService.IsAuctionValid(Id))
            {
                return BadRequest();
            }

            return View(new ListingsFormModel
            {
                Conditions = await listingService.Conditions(),
                Types = await listingService.Types(),
                Makes = await listingService.Makes(DefaultTypeId),
                Models = await listingService.Models(DefaultTypeId, DefaulMakeId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Id, ListingsFormModel listing)
        {
            if (!await validatorService.AreListingParamsValid(listing.ConditionId,
                listing.ModelId,
                listing.MakeId,
                listing.TypeId))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                listing.Conditions = await listingService.Conditions();
                listing.Types = await listingService.Types();
                listing.Makes = await listingService.Makes(DefaultTypeId);
                listing.Models = await listingService.Models(DefaultTypeId, DefaulMakeId);

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

            TempData[SuccessMessageKey] = "Listing was created successfully and awaits approval!";

            return RedirectToAction(nameof(Mine));
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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

            var listing = await listingService.Details(listingId, auctionId);

            if (listing == null)
            {
                return BadRequest();
            }

            var bid = await bidService.HighestForListing(listingId);

            var listingViewModel = new ListingViewModel
            {
                Auction = auction,
                Id = listing.Id,
                UserId = listing.UserId,
                Type = listing.Type,
                Condition = listing.Condition,
                Make = listing.Make,
                Model = listing.Model,
                Price = listing.Price.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
                Year = listing.Year,
                Description = listing.Description,
                BidUser = bid == null ? "None" : bid.User,
                BidAmount = bid == null ? "0" : bid.Amount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
                IsWatched = await listingService.IsWatched(listingId, this.User.Id())
            };

            return View(listingViewModel);
        }


        public async Task<IActionResult> Mine()
        {
            var myListings = await listingService.Mine(this.User.Id());

            return View(myListings);
        }
    }
}
