namespace AMS.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using AMS.Controllers.Models;

    using AMS.Services.Contracts;

    public class HomeController(
        IStatisticService statisticService,
        IListingService listingService,
        IMemoryCache cache)
        : Controller
    {
        public async Task<IActionResult> Index()
        {
            const string HomeViewKey = "HomeViewKey";

            var model = cache.Get<HomeViewModel>(HomeViewKey);

            if (model == null)
            {
                model = new HomeViewModel
                {
                    Statistics = await statisticService.Total(),
                    Preview = await listingService.Preview()
                };

                cache.Set(HomeViewKey,
                    model,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(15)));
            }

            return View(model);
        }
    }
}