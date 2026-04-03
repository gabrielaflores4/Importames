using Importames.Data;
using Importames.Models;
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

        public IActionResult Edit(int id)
        {
            var vehiculo = _context.Vehiculos.Find(id);
            return View("EditarVehiculo", vehiculo);
        }

        [HttpPost]
        public IActionResult Edit(VehiculoModel vehiculo)
        {
            _context.Vehiculos.Update(vehiculo);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
