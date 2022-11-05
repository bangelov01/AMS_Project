namespace AMS.Tests.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using Xunit;
    using Moq;

    using AMS.Controllers;
    using AMS.Controllers.Models.Listings;

    using AMS.Services;
    using AMS.Services.Models.Listings;

    using AMS.Tests.Mocks;
    using AMS.Tests.Database;

    using static AMS.Tests.Mocks.ControllerContextMock;

    public class ListingsTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture fixture;

        private readonly IMapper mapper;
        private readonly ListingsController listingsController;

        private const string auctionTestId = "TestAuctionId0";

        public ListingsTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
            this.mapper = MapperMock.Instance;

            this.listingsController = new ListingsController(new ListingService(fixture.data, mapper),
                new ValidatorService(fixture.data),
                new AuctionService(fixture.data, new AddressService(fixture.data), mapper),
                new BidService(fixture.data));

            this.listingsController.ControllerContext = ControllerContextInstance("TestUsername0", "TestUser0");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(2.2)]
        public async Task GetMakes_ReturnsBadRequest_WithInvalidTypeId(int value)
        {
            var result = await listingsController.GetMakes(value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetMakes_ReturnsOkResult_WithValidTypeId()
        {
            var result = await listingsController.GetMakes(1);

            Assert.NotNull(result);
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ICollection<ListingPropertyServiceModel>>(viewResult.Value);

            Assert.Single(model);
            Assert.Equal(1, model.Single().Id);
            Assert.Equal("TestMake", model.Single().Name);
        }

        [Theory]
        [InlineData(0,0)]
        [InlineData(-1,0)]
        [InlineData(0,-1)]
        [InlineData(2.2,2.3)]
        public async Task GetModels_ReturnsBadRequest_WithInvalidTypeIdAndMakeId(int type, int make)
        {
            var result = await listingsController.GetModels(type,make);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetModels_ReturnsOkResult_WithValidTypeIdAndMakeId()
        {
            var result = await listingsController.GetModels(1,1);

            Assert.NotNull(result);
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ICollection<ListingPropertyServiceModel>>(viewResult.Value);

            Assert.Single(model);
            Assert.Equal(1, model.Single().Id);
            Assert.Equal("TestModel", model.Single().Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        public async Task Create_ReturnsBadRequest_WithInvalidAuctionId(string value)
        {
            var result = await listingsController.Create(value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsViewResultAndDetails_WithValidAuctionId()
        {
            var result = await listingsController.Create(auctionTestId);

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ListingsFormModel>(viewResult.Model);

            Assert.Single(model.Conditions);
            Assert.Single(model.Makes);
            Assert.Single(model.Models);
            Assert.Equal(4,model.Types.Count);
        }

        [Theory]
        [InlineData(10,10,10,10)]
        [InlineData(-1,-1,-1,-1)]
        public async Task CreatePost_ReturnsBadRequest_WithInvalidDetailIds(int condition, int model, int type, int make)
        {
            var testForm = new ListingsFormModel { ConditionId = condition, ModelId = model, TypeId = type, MakeId = make };

            var result = await listingsController.Create(auctionTestId, testForm);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreatePost_ReturnsViewResult_WithInvalidModelState()
        {
            listingsController.ModelState.AddModelError("test", "test");

            var testForm = new ListingsFormModel { ConditionId = 1, ModelId = 1, TypeId = 1, MakeId = 1 };

            var result = await listingsController.Create(auctionTestId, testForm);

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(!listingsController.ModelState.IsValid);
            Assert.StrictEqual(testForm, viewResult.Model);
        }

        [Fact]
        public async Task CreatePost_ReturnsRedirectToActionResult_AndCreatesListing_WithValidData()
        {
            var currentCount = await fixture.data.Vehicles.CountAsync();

            listingsController.TempData = Mock.Of<ITempDataDictionary>();

            var testForm = new ListingsFormModel { ConditionId = 1, ModelId = 1, TypeId = 1, MakeId = 1 };

            var result = await listingsController.Create(auctionTestId, testForm);

            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("Mine", redirectResult.ActionName);
            Assert.NotEqual(currentCount, await fixture.data.Vehicles.CountAsync());
        }

        [Fact]
        public async Task Mine_ReturnsViewResult_WithMyListingsServiceModel()
        {
            var result = await listingsController.Mine();

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<MyListingsServiceModel>>(viewResult.Model);

            Assert.Equal(6, model.Count());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task All_ReturnsBadRequest_WithNullOrEmptyAuctionId(string value)
        {
            var result = await listingsController.All(value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task All_ReturnsNoResultViewResult_WithInvalidAuctionId()
        {
            var result = await listingsController.All("invalid");

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("NoResult", viewResult.ViewName);
        }

        [Fact]
        public async Task All_ReturnsBadRequest_WithInvalidCurrentPage()
        {
            var result = await listingsController.All(auctionTestId, 10);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task All_ReturnsViewResponse_WithValidData_AndAllListingsViewModel()
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Listings");
            listingsController.ControllerContext = new ControllerContext { RouteData = routeData };

            var result = await listingsController.All(auctionTestId, 1);

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AllListingsViewModel>(viewResult.Model);

            Assert.NotNull(model.Listings);
            Assert.Equal(3, model.Listings.Count());

            Assert.NotNull(model.Auction);
            Assert.Equal(auctionTestId, model.Auction.Id);

            Assert.NotNull(model.Pagination);

            Assert.Equal(1, model.Pagination.CurrentPage);
            Assert.Equal(3, model.Pagination.ItemsCount);
            Assert.Equal(2, model.Pagination.MaxPage);
            Assert.Equal("Listings", model.Pagination.ControllerName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task Find_ReturnsBadRequest_WithNullOrEmptySearchTerm(string value)
        {
            var result = await listingsController.Find(value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData(null)]
        public async Task Find_ReturnsBadRequest_WithInvalidOrderTerm(string value)
        {
            var result = await listingsController.Find("test", value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Find_ReturnsNoResultView_WithUnmatchingSearchTerm()
        {
            var result = await listingsController.Find("invalidSearch");

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("NoResult", viewResult.ViewName);
        }

        [Fact]
        public async Task Find_ReturnsViewResult_WithValidData_AndSearchListingViewModel()
        {
            var result = await listingsController.Find("test");

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<SearchListingsViewModel>(viewResult.Model);

            Assert.NotNull(model.Listings);
            Assert.Equal(5, model.Listings.Count());
            Assert.Equal("test", model.SearchTerm);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task Details_ReturnsBadRequest_WithNullORemptyAuctionIdOrListingId(string value)
        {
            var result = await listingsController.Details(value, value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsBadRequest_WithNonMatchingAuctionIdAndListingId()
        {
            var result = await listingsController.Details(auctionTestId, "invalidListingId");

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNoResultView_WithNonExistantAuction()
        {
            var result = await listingsController.Details("invalidAuctionId", "test");

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("NoResult", viewResult.ViewName);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithValidData_AndListingViewModel()
        {
            var result = await listingsController.Details(auctionTestId, "TestVehicleId0");

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ListingViewModel>(viewResult.Model);

            Assert.NotNull(model.Auction);

            Assert.Equal(auctionTestId, model.Auction.Id);
            Assert.Equal("TestVehicleId0", model.Id);
            Assert.Equal("104.00", model.BidAmount);
            Assert.True(model.IsWatched);
        }
    }
}
