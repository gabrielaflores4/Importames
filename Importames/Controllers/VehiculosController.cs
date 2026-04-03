using Importames.Data;
using Importames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Importames.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly AppDbContext _context;
        public VehiculosController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? estadoId)
        {
            var query = _context.Vehiculos
                .Include(v => v.Estado)
                .Include(v => v.Cliente)
                .AsQueryable();

            if (estadoId != null)
            {
                query = query.Where(v => v.IdEstado == estadoId);
            }

            ViewBag.Estados = _context.Estados.ToList();

            return View("VehiculosView", query.ToList());
        }

        [HttpPost]
        public IActionResult Create(VehiculoModel v)
        {
            v.FechaIngreso = DateTime.Now;

            _context.Vehiculos.Add(v);
            _context.SaveChanges();

            var historial = new HistorialEstadoModel
            {
                IdVehiculo = v.IdVehiculo,
                IdEstado = v.IdEstado,
                IdUsuario = 1,
                FechaCambio = DateTime.Now
            };

            _context.Historiales.Add(historial);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            ViewBag.Clientes = _context.Clientes.ToList();
            ViewBag.Estados = _context.Estados.ToList();

            return View("CrearVehiculo");
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
