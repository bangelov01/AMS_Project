namespace AMS.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using AMS.Services.Contracts;
    using AMS.Web.Areas.Admin.Controllers.Base;

    public class UsersController : AdminController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult All()
        {
            var users = userService.All();

            return View(users);
        }

        public IActionResult Suspend(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return BadRequest();
            }

            if (!userService.Suspend(Id))
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All), "Users");
        }

        public IActionResult Allow(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return BadRequest();
            }

            if (!userService.Allow(Id))
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All), "Users");
        }
    }
}
