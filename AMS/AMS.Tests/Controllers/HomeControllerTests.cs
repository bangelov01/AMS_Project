namespace AMS.Tests.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using Xunit;

    using AMS.Controllers;
    using AMS.Controllers.Models;

    using AMS.Services;

    using AMS.Tests.Database;
    using AMS.Tests.Mocks;

    public class HomeControllerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture fixture;

        public HomeControllerTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task IndexShouldReturnViewWithCorrectModel()
        {
            var mapper = MapperMock.Instance;

            var listingsService = new ListingService(fixture.data, mapper);
            var statisticsService = new StatisticsService(fixture.data);

            var homeController = new HomeController(statisticsService,
                listingsService, new MemoryCache(new MemoryCacheOptions()));

            var result = await homeController.Index();

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<HomeViewModel>(model);

            Assert.Equal(3, indexViewModel.Preview.Count());
            Assert.Equal(5, indexViewModel.Statistics.TotalListings);
            Assert.Equal(1, indexViewModel.Statistics.TotalAuctions);
            Assert.Equal(1, indexViewModel.Statistics.TotalUsers);
        }
    }
}
