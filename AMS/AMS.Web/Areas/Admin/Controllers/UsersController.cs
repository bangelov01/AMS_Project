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
            userService.Suspend(Id);

            return RedirectToAction(nameof(All), "Users");
        }

        public IActionResult Allow(string Id)
        {
            userService.Allow(Id);

            return RedirectToAction(nameof(All), "Users");
        }
    }
}
