namespace AMS.Web.Areas.Admin.Controllers
{
    using AMS.Web.Areas.Admin.Controllers.Base;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
