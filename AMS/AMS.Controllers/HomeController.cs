namespace AMS.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using AMS.Controllers.Models;

    using AMS.Services.Contracts;
    using AMS.Services.Models.Listings.Base;

    public class HomeController : Controller
    {
        private readonly IStatisticService statisticService;
        private readonly IListingService listingService;
        private readonly IMemoryCache cache;

        public HomeController(IStatisticService statisticService,
            IListingService listingService,
            IMemoryCache cache)
        {
            this.statisticService = statisticService;
            this.listingService = listingService;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            const string PreviewListingsKey = "PreviewListingsKey";

            var previewListings = this.cache.Get<IEnumerable<ListingsServiceModel>>(PreviewListingsKey);

            if (previewListings == null)
            {
                previewListings = await listingService.Preview();

                this.cache.Set(PreviewListingsKey,
                    previewListings,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
            }

            var statistics = await statisticService.Total();

            return View(new HomeViewModel
            {
                Preview = previewListings,
                Statistics = statistics
            });
        }
    }
}