namespace AMS.Controllers
{
    using AMS.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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

        public IActionResult All()
        {
            var userListings = watchlistService.ListingsForUser(this.User.Id());

            return View(userListings);
        }

        public IActionResult Watch(string Id)
        {
            if (string.IsNullOrEmpty(Id) || validatorService.DoesWatchlistExist(Id, this.User.Id()))
            {
                return BadRequest();
            }

            watchlistService.Create(Id, this.User.Id());

            return RedirectToAction(nameof(All));
        }

        public IActionResult Delete(string Id)
        {
            bool isDeleted = watchlistService.Delete(Id, this.User.Id());

            if (!isDeleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }
    }
}
