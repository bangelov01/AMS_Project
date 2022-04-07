namespace AMS.Tests.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using Xunit;

    using AMS.Controllers;
    using AMS.Controllers.Models;

    using AMS.Data;

    using AMS.Services;

    using AMS.Tests.Mocks;

    using static AMS.Tests.Database.DatabaseInitialize;

    public class AuctionsControllerTests : IDisposable
    {
        private readonly AMSDbContext data;
        private readonly IMapper mapper;
        private readonly AddressService addressService;
        private readonly AuctionService auctionService;
        private readonly AuctionsController auctionsController;

        public AuctionsControllerTests()
        {
            this.data = Initialize();
            this.mapper = MapperMock.Instance;
            this.addressService = new AddressService(data);
            this.auctionService = new AuctionService(data, addressService, mapper);
            this.auctionsController = new AuctionsController(auctionService);
        }

        public void Dispose()
        {
            this.data.Dispose();
        }

        [Fact]
        public async Task All_ReturnsViewResult_WithAllAuctionsViewModel()
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Auctions");
            auctionsController.ControllerContext = new ControllerContext { RouteData = routeData };

            var result = await auctionsController.All();

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = viewResult.Model;

            var allViewModel = Assert.IsType<AllAuctionsViewModel>(model);

            Assert.NotNull(allViewModel.Auctions);
            Assert.Equal(2, allViewModel.Auctions.Count());

            Assert.Equal(2, allViewModel.Total);

            Assert.NotNull(allViewModel.Pagination);

            Assert.Equal(1, allViewModel.Pagination.CurrentPage);
            Assert.Equal(2, allViewModel.Pagination.ItemsCount);
            Assert.Equal(1, allViewModel.Pagination.MaxPage);
            Assert.Null(allViewModel.Pagination.Id);
            Assert.Equal("Auctions", allViewModel.Pagination.ControllerName);
        }

        [Fact]
        public async Task All_ReturnsNoResultViewResult_WithoutActiveAuctions()
        {
            var auct = await data.Auctions.ToArrayAsync();
            data.Auctions.RemoveRange(auct);
            data.SaveChangesAsync();

            var result = await auctionsController.All();

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("NoResult", viewResult.ViewName);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(40)]
        public async Task All_ReturnsBadRequestResult_WithInvalidCurrentPage(int value)
        {
            var result = await auctionsController.All(value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
