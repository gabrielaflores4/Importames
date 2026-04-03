using Importames.Data;
using Microsoft.AspNetCore.Mvc;

namespace Importames.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly AppDbContext _context;
        public VehiculosController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var lista = _context.Vehiculos.ToList();
            return View("VehiculosView", lista);
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

        [HttpPost]
        public IActionResult Edit()
        {
            return RedirectToAction("Index");
        }
    }
}
