namespace AMS.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using AMS.Controllers.Models;

    using AMS.Services.Contracts;

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
            const string HomeViewKey = "HomeViewKey";

            var model = this.cache.Get<HomeViewModel>(HomeViewKey);

            if (model == null)
            {
                model = new HomeViewModel
                {
                    Statistics = await statisticService.Total(),
                    Preview = await listingService.Preview()
                };

                this.cache.Set(HomeViewKey,
                    model,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(15)));
            }

            return View(model);
        }
    }
}