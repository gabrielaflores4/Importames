using Microsoft.AspNetCore.Mvc;

namespace Importames.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
