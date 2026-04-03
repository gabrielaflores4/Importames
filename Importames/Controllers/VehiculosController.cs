using Microsoft.AspNetCore.Mvc;

namespace Importames.Controllers
{
    public class VehiculosController : Controller
    {
        public IActionResult Index()
        {
            return View("VehiculosView");
        }

        public IActionResult Create()
        {
            return View("CrearVehiculo");
        }

        [HttpPost]
        public IActionResult Create(string nombre, string color)
        {
            return RedirectToAction("Index");
        }
    }
}
