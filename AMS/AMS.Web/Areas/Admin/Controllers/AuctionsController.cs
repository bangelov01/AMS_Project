namespace AMS.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using AMS.Services.Contracts;

    using AMS.Web.Areas.Admin.Controllers.Base;
    using AMS.Web.Areas.Admin.Models;

    public class AuctionsController : AdminController
    {
        private readonly IAuctionService auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            this.auctionService = auctionService;
        }

        public IActionResult Create()
            => View();

        [HttpPost]
        public async Task<IActionResult> Create(AuctionFormModel auction)
        {
            if (await auctionService.IsCreated(auction.Number))
            {
                this.ModelState.AddModelError(nameof(auction.Number), "Auction with that number already exists!");
            }

            if (!ModelState.IsValid)
            {
                return View(auction);
            }

            await auctionService.Create(auction.Number,
                auction.Description,
                auction.Start,
                auction.End,
                auction.Country,
                auction.City,
                auction.AddressText);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> All() 
            => View(await auctionService.All());

        public async Task<IActionResult> Edit(string Id)
        {
            var auction = await auctionService.AdminDetailsById(Id);

            if (auction == null)
            {
                return BadRequest();
            }

            var auctionForm = new AuctionFormModel
            {
                Number = auction.Number,
                Description = auction.Description,
                Start = auction.Start,
                End = auction.End,
                Country = auction.Country,
                City = auction.City,
                AddressText = auction.AddressText
            };

            return View(auctionForm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string Id, AuctionFormModel auction)
        {
            if (!ModelState.IsValid)
            {
                return View(auction);
            }

            await auctionService.Edit(Id,
                auction.Number,
                auction.Description,
                auction.Start,
                auction.End,
                auction.Country,
                auction.City,
                auction.AddressText);

            return RedirectToAction(nameof(All), "Auctions", new { area = "Admin"});
        }
    }
}
