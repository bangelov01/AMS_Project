namespace AMS.Tests.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    using AMS.Data;

    using AMS.Services;
    using AMS.Services.Models.Listings;

    using AMS.Web.Areas.Admin.Controllers;

    using AMS.Tests.Mocks;
    using static AMS.Tests.Database.DatabaseInitialize;

    public class HomeTests : IDisposable
    {
        private readonly AMSDbContext data;

        private readonly HomeController homeController;

        private const string listingTestId = "TestVehicleId0";

        public HomeTests()
        {
            this.data = Initialize();
            this.homeController = new HomeController(new ListingService(data, MapperMock.Instance));
        }

        public void Dispose()
        {
            this.data.Dispose();
        }

        [Fact]
        public async Task Index_ShouldReturnViewResult_WithNotApprovedListings()
        {
            var listing = await data.Vehicles.FindAsync(listingTestId);
            listing.IsApproved = false;
            await data.SaveChangesAsync();

            var result = await homeController.Index();

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<AdminListingsServiceModel>>(viewResult.Model);

            Assert.Equal(1, model.Count());
            Assert.Equal(listingTestId, model.First().Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        public async Task Approve_ReturnsBadRequest_WithNullOrEmptyListingId_OrNonExistantId(string value)
        {
            var result = await homeController.Approve(value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Approve_ReturnsRedirectToActionResult_AndApprovesListing()
        {
            var listing = await data.Vehicles.FindAsync(listingTestId);
            listing.IsApproved = false;
            await data.SaveChangesAsync();

            var result = await homeController.Approve(listingTestId);

            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.True(listing.IsApproved);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        public async Task Delete_ReturnsBadRequest_WithNullOrEmptyListingId_OrNonExistantId(string value)
        {
            var currentCount = await data.Vehicles.CountAsync();

            var result = await homeController.Delete(value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsRedirectToActionResult_AndDeletesListing()
        {
            var currentCount = await data.Vehicles.CountAsync();

            var result = await homeController.Delete(listingTestId);

            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.NotEqual(currentCount, data.Vehicles.Count());
        }
    }
}
