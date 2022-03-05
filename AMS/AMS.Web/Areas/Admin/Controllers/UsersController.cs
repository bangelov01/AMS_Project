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
            var users = userService.AllUsers();

            return View(users);
        }

        public IActionResult Suspend(string Id)
        {
            userService.SuspendUser(Id);

            return RedirectToAction(nameof(All), "Users");
        }

        public IActionResult Allow(string Id)
        {
            userService.AllowUser(Id);

            return RedirectToAction(nameof(All), "Users");
        }
    }
}
