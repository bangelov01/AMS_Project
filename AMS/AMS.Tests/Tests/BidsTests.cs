namespace AMS.Tests.Tests
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

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
        public async Task Create_ReturnsRedirectToActionResult_AndCreatesBid()
        {
            var result = await bidsController.Create(auctionTestId, listingTestId, new BidInfoModel { Amount = 1100 });

            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
