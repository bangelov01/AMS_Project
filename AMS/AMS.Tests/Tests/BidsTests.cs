namespace AMS.Tests.Tests
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    using AMS.Controllers;
    using AMS.Controllers.Models;

    using AMS.Data;

    using AMS.Services;

    using AMS.Tests.Mocks;
    using static AMS.Tests.Database.DatabaseInitialize;
    using static AMS.Tests.Mocks.ControllerContextMock;

    public class BidsTests : IDisposable
    {
        private readonly AMSDbContext data;
        private readonly BidsController bidsController;

        private const string auctionTestId = "TestAuctionId0";
        private const string listingTestId = "TestVehicleId0";

        public BidsTests()
        {
            this.data = Initialize();

            this.bidsController = new BidsController(new BidService(data),
                new ValidatorService(data),
                HubContextMock.Instance);

            this.bidsController.ControllerContext = ControllerContextInstance("TestUsername1", "TestUser1");
        }

        public void Dispose()
        {
            this.data.Dispose();
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WithInvalidModelState()
        {
            bidsController.ModelState.AddModelError("test", "test");

            var result = await bidsController.Create(auctionTestId, listingTestId, new BidInfoModel { Amount = 110 });

            Assert.NotNull(result);
            Assert.True(!bidsController.ModelState.IsValid);
            Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData("invalid", 50)]
        [InlineData(listingTestId, 50)]
        [InlineData("invalid", 200)]
        [InlineData(null, 0)]
        public async Task Create_ReturnsBadRequest_WithInvalidListingOrBidAmount(string listingId, decimal bidAmount)
        {
            var result = await bidsController.Create(auctionTestId, listingId, new BidInfoModel { Amount = bidAmount });

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WithIncomingAmountLowerThanHighestBid()
        {
            var result = await bidsController.Create(auctionTestId, listingTestId, new BidInfoModel { Amount = 102 });

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsRedirectToActionResult_AndCreatesBid()
        {
            var currentBidCount = await data.Bids.CountAsync();

            var result = await bidsController.Create(auctionTestId, listingTestId, new BidInfoModel { Amount = 1100 });

            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Listings", redirectResult.ControllerName);
            Assert.Equal("Details", redirectResult.ActionName);
            Assert.Equal(auctionTestId, redirectResult.RouteValues["auctionId"]);
            Assert.Equal(listingTestId, redirectResult.RouteValues["listingId"]);
            Assert.NotEqual(currentBidCount, await data.Bids.CountAsync());
        }
    }
}
