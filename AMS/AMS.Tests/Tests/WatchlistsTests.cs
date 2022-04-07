namespace AMS.Tests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using Xunit;

    using AMS.Controllers;

    using AMS.Data;

    using AMS.Services;
    using AMS.Services.Models.Listings;

    using AMS.Tests.Mocks;
    using static AMS.Tests.Database.DatabaseInitialize;
    using static AMS.Tests.Mocks.ControllerContextMock;

    public class WatchlistsTests : IDisposable
    {
        private readonly AMSDbContext data;

        private readonly WatchlistsController watchlistsController;
        public WatchlistsTests()
        {
            this.data = Initialize();
            this.watchlistsController = new WatchlistsController(new WatchlistService(data, MapperMock.Instance),
                new ValidatorService(data));

            this.watchlistsController.ControllerContext = ControllerContextInstance("TestUsername0", "TestUser0");
        }

        public void Dispose()
        {
            this.data.Dispose();
        }

        [Fact]
        public async Task All_ReturnsViewResultAndListings_WithSearchListingsServiceModel()
        {
            var result = await watchlistsController.All();

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SearchListingsServiceModel>>(viewResult.Model);

            Assert.Equal(2, model.Count());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Watch_ReturnsBadRequest_WithNullOrEmptyListingId(string value)
        {
            var result = await watchlistsController.Watch(value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Watch_ReturnsBadRequestResult_WithAlreadyExistingWatchlist()
        {
            var result = await watchlistsController.Watch("TestVehicleId0");

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Watch_ReturnsRedirectToActionResult_AndCreatesWatchlist()
        {
            var currentCount = await data.Watchlists.CountAsync();

            var result = await watchlistsController.Watch("TestVehicleId3");

            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("All", redirectResult.ActionName);
            Assert.NotEqual(currentCount, await data.Watchlists.CountAsync());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Delete_ReturnsBadRequest_WithNullOrEmptyListingId(string value)
        {
            var result = await watchlistsController.Delete(value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WithInvalidListingId()
        {
            var result = await watchlistsController.Delete("invalid");

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsRedirectToActionResult_AndDeletesWatchlist()
        {
            var currentCount = await data.Watchlists.CountAsync();

            var result = await watchlistsController.Delete("TestVehicleId0");

            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("All", redirectResult.ActionName);
            Assert.NotEqual(currentCount, await data.Watchlists.CountAsync());
        }
    }
}
