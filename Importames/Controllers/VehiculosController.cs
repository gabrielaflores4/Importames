using Microsoft.AspNetCore.Mvc;

namespace Importames.Controllers
{
    public class VehiculosController : Controller
    {
        public IActionResult Index()
        {
            return View("VehiculosView");
        }
    }
}
