namespace AMS.Controllers
{
    using AMS.Controllers.Models;
    using AMS.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static AMS.Services.Infrastructure.Extensions.ClaimsPrincipleExtensions;

    [Authorize]
    public class BidsController : Controller
    {
        private readonly IBidService bidService;
        private readonly IValidatorService validatorService;

        public BidsController(IBidService bidService,
            IValidatorService validatorService)
        {
            this.bidService = bidService;
            this.validatorService = validatorService;
        }

        public IActionResult GetBid(string listingId)
        {
            if (string.IsNullOrEmpty(listingId))
            {
                return BadRequest();
            }

            var bid = bidService.HighestForListing(listingId);

            return Json(bid);
        }

        [HttpPost]
        public IActionResult PostBid(BidInfoModel bid)
        {
            
            if (!ModelState.IsValid || !validatorService.IsListingValid(bid.ListingId))
            {
                return BadRequest();
            }

            var highestBid = bidService.HighestForListing(bid.ListingId);

            if (highestBid != null && highestBid.Amount >= bid.Amount)
            {
                return BadRequest();
            }

            Random random = new Random();

            var number = random.Next(1000, 5000);

            bidService.Create(this.User.Id(), bid.ListingId, bid.Amount, number);

            return Json(new { Success = true });
        }
    }
}
