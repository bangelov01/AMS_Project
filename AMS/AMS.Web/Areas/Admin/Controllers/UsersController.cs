namespace AMS.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using AMS.Services.Contracts;
    using AMS.Web.Areas.Admin.Controllers.Base;

    public class UsersController(IUserService userService) : AdminController
    {
        public async Task<IActionResult> All() 
            => View(await userService.All());

        public async Task<IActionResult> Suspend(string Id)
        {

            if (string.IsNullOrEmpty(Id) || !await userService.Suspend(Id))
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Allow(string Id)
        {
            if (string.IsNullOrEmpty(Id) || !await userService.Allow(Id))
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }
    }
}
