using Microsoft.AspNetCore.Mvc;

namespace EPYTST.API.Contollers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            if (TempData["User"] != null)
            {
                var userJson = TempData["User"].ToString();
                ViewBag.User = userJson;
            }

            return View();
        }
    }
}
