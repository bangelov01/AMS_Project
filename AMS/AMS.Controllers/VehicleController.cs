namespace AMS.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class VehicleController : Controller
    {

        [Authorize]
        public IActionResult Create()
        { 
            return View(); 
        }
    }
}
