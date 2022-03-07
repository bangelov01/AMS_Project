﻿namespace AMS.Web.Areas.Admin.Controllers
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
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AuctionFormModel auction)
        {
            if (auctionService.IsAuctionCreated(auction.Number))
            {
                this.ModelState.AddModelError(nameof(auction.Number), "Auction with that number already exists!");
            }

            if (!ModelState.IsValid)
            {
                return View(auction);
            }

            auctionService.CreateAuction(auction.Number,
                auction.Description,
                auction.Start,
                auction.End,
                auction.Country,
                auction.City,
                auction.AddressText);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult All()
        {
            var auctions = auctionService.ActiveAuctions();

            return View(auctions);
        }

        public IActionResult Edit(string Id)
        {
            var auction = auctionService.AuctionById(Id);

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
        public IActionResult Edit(string Id, AuctionFormModel auction)
        {
            auctionService.Edit(Id,
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
