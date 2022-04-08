namespace AMS.Tests.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Xunit;

    using AMS.Services;
    using AMS.Services.Models.Auctions;

    using AMS.Data;

    using AMS.Web.Areas.Admin.Controllers;
    using AMS.Web.Areas.Admin.Models;

    using AMS.Tests.Mocks;
    using static AMS.Tests.Database.DatabaseInitialize;

    public class AuctionsTests : IDisposable
    {
        private readonly AMSDbContext data;
        private readonly AuctionsController auctionsController;

        private const string auctionTestId = "TestAuctionId0";

        public AuctionsTests()
        {
            this.data = Initialize();
            this.auctionsController = new AuctionsController(new AuctionService(data, new AddressService(data), MapperMock.Instance));
        }

        public void Dispose()
        {
            this.data.Dispose();
        }

        [Fact]
        public async Task Create_ReturnsViewResult()
        {
            var result = auctionsController.Create();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CreatePost_ReturnsSameForm_WithInvalidState()
        {
            var testForm = new AuctionFormModel{ Number = 1 };

            var result = await auctionsController.Create(testForm);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.True(!auctionsController.ViewData.ModelState.IsValid);
            Assert.StrictEqual(testForm, viewResult.Model);
        }

        [Fact]
        public async Task CreatePost_CreatesEntity_AndReturnsRedirectToActionResult()
        {
            var testForm = new AuctionFormModel { Number = 40 };

            var result = await auctionsController.Create(testForm);

            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("All", redirectResult.ActionName);
            Assert.Equal(testForm.Number, data.Auctions.Last().Number);
        }

        [Fact]
        public async Task All_ReturnsViewResult_WithAListOfAllAuctionsServiceModel()
        {
            var result = await auctionsController.All();

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<AllAuctionsServiceModel>>(viewResult.Model);

            Assert.Equal(2, model.Count());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("nonexistant")]
        public async Task Edit_ReturnsBadRequest_WithInvalidAuctionId(string value)
        {
            var result = await auctionsController.Edit(value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithAuctionFormModel()
        {
            var result = await auctionsController.Edit(auctionTestId);

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var editViewModel = Assert.IsType<AuctionFormModel>(viewResult.Model);

            Assert.Equal(0, editViewModel.Number);
            Assert.Equal("TestDescription", editViewModel.Description);
            Assert.Equal("TestCity", editViewModel.City);
        }

        [Fact]
        public async Task EditPost_EditsEntity_AndReturnsRedirectToActionResult()
        {
            var testForm = new AuctionFormModel{ Number = 40 };

            var result = await auctionsController.Edit(auctionTestId, testForm);

            Assert.NotNull(result);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Auctions", redirectResult.ControllerName);
            Assert.Equal("All", redirectResult.ActionName);
            Assert.Equal("Admin", redirectResult.RouteValues["area"]);
            Assert.Equal(testForm.Number, data.Auctions.Find(auctionTestId).Number);
        }

        [Fact]
        public async Task EditPost_ReturnsSameForm_WithInvalidState()
        {
            var testForm = new AuctionFormModel();

            auctionsController.ModelState.AddModelError("test", "test");

            var result = await auctionsController.Edit(auctionTestId, testForm);

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.True(!auctionsController.ViewData.ModelState.IsValid);
            Assert.StrictEqual(testForm, viewResult.Model);
        }
    }
}
