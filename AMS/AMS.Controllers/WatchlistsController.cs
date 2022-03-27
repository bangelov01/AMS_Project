namespace AMS.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using AMS.Services.Contracts;

    using static AMS.Services.Infrastructure.Extensions.ClaimsPrincipleExtensions;

    [Authorize]
    public class WatchlistsController : Controller
    {
        private readonly IWatchlistService watchlistService;
        private readonly IValidatorService validatorService;

        public WatchlistsController(IWatchlistService watchlistService,
            IValidatorService validatorService)
        {
            this.watchlistService = watchlistService;
            this.validatorService = validatorService;
        }

        public async Task<IActionResult> All() 
            => View(await watchlistService.ListingsForUser(this.User.Id()));

        public async Task<IActionResult> Watch(string Id)
        {
            if (string.IsNullOrEmpty(Id) || await validatorService.DoesWatchlistExist(Id, this.User.Id()))
            {
                return BadRequest();
            }

            await watchlistService.Create(Id, this.User.Id());

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return BadRequest();
            }

            bool isDeleted = await watchlistService.Delete(Id, this.User.Id());

            if (!isDeleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }
    }
}
