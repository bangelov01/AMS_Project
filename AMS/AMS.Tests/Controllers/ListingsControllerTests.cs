namespace AMS.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
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

    public class ListingsControllerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture fixture;

        private readonly IMapper mapper;
        private readonly AddressService addressService;
        private readonly AuctionService auctionService;
        private readonly ValidatorService validatorService;
        private readonly ListingService listingService;
        private readonly BidService bidService;

        private readonly ListingsController listingsController;

        private const string auctionTestId = "TestAuctionId0";

        public ListingsControllerTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;

            this.mapper = MapperMock.Instance;
            this.addressService = new AddressService(fixture.data);
            this.auctionService = new AuctionService(fixture.data, addressService, mapper);
            this.validatorService = new ValidatorService(fixture.data);
            this.listingService = new ListingService(fixture.data, mapper);
            this.bidService = new BidService(fixture.data);

            this.listingsController = new ListingsController(listingService,
                validatorService,
                auctionService,
                bidService);
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
        [InlineData(10,10)]
        [InlineData(-1,-1)]
        [InlineData(10,-1)]
        [InlineData(-1,10)]
        public async Task CreatePost_ReturnsBadRequest_WithInvalidConditionIdOrModelId(int condition, int model)
        {
            var testForm = new ListingsFormModel { ConditionId = condition, ModelId = model };

            var result = await listingsController.Create(auctionTestId, testForm);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreatePost_ReturnsViewResult_WithInvalidModelState()
        {
            listingsController.ModelState.AddModelError("test", "test");

            var testForm = new ListingsFormModel { ConditionId = 1, ModelId = 1 };

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

            var claimsMock = Mock.Of<ClaimsPrincipal>();

            listingsController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsMock }
            };

            listingsController.TempData = Mock.Of<ITempDataDictionary>();

            var testForm = new ListingsFormModel { ConditionId = 1, ModelId = 1, Description = "newListing" };

            var result = await listingsController.Create(auctionTestId, testForm);

            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("Mine", redirectResult.ActionName);
            Assert.NotEqual(currentCount, await fixture.data.Vehicles.CountAsync());
            Assert.True(await fixture.data.Vehicles.AnyAsync(v => v.Description == "newListing"));
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
            var claimsMock = Mock.Of<ClaimsPrincipal>();

            listingsController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsMock }
            };

            var result = await listingsController.Details(auctionTestId, "TestVehicleId0");

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ListingViewModel>(viewResult.Model);

            Assert.NotNull(model.Auction);
            Assert.NotNull(model.Listing);
            Assert.NotNull(model.Bid);

            Assert.Equal(auctionTestId, model.Auction.Id);
            Assert.Equal("TestVehicleId0", model.Listing.Id);
            Assert.Equal(104, model.Bid.Amount);
            Assert.True(!model.IsWatched);
        }

        [Fact]
        public async Task Mine_ReturnsViewResult_WithMyListingsServiceModel()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "TestUsername0"),
                new Claim(ClaimTypes.NameIdentifier, "TestUser0")

            }, "mock"));

            listingsController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var result = await listingsController.Mine();

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<MyListingsServiceModel>>(viewResult.Model);

            Assert.Equal(5, model.Count());
        }
    }
}
