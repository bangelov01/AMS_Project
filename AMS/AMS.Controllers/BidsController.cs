namespace AMS.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    using AMS.Controllers.Hubs;
    using AMS.Controllers.Models;

    using AMS.Services.Contracts;
    using static AMS.Services.Infrastructure.Extensions.ClaimsPrincipleExtensions;

    [Authorize]
    public class BidsController(
        IBidService bidService,
        IValidatorService validatorService,
        IHubContext<BidHub> hubContext)
        : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create(string auctionId, string listingId, BidInfoModel bid)
        {
            if (!ModelState.IsValid || !await validatorService.IsListingValidForBid(listingId, this.User.Id(), bid.Amount))
            {
                return BadRequest();
            }

            var highestBid = await bidService.HighestForListing(listingId);

            if (highestBid != null && highestBid.Amount >= bid.Amount)
            {
                return BadRequest();
            }

            await hubContext.Clients.All.SendAsync("onBid", bid.Amount.ToString("f2"),
                this.User.Identity.Name,
                listingId);

            Random random = new Random();

            var number = random.Next(1000, 5000);

            await bidService.Create(this.User.Id(), listingId, bid.Amount, number);

            return RedirectToAction("Details", "Listings", new
            {
                auctionId, listingId
            });
        }
    }
}
