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
        [IgnoreAntiforgeryToken]
        public IActionResult Create(VehiculoModel v)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(v.Marca) && string.IsNullOrWhiteSpace(v.Modelo) &&
                    string.IsNullOrWhiteSpace(v.Color) && string.IsNullOrWhiteSpace(v.Vin) &&
                    v.Costo == 0 && v.IdCliente == 0)
                    return Json(new { exito = false, mensaje = "Por favor complete el formulario antes de guardar." });

                // Validaciones individuales
                if (string.IsNullOrWhiteSpace(v.Marca))
                    return Json(new { exito = false, mensaje = "La marca es requerida." });
                if (string.IsNullOrWhiteSpace(v.Modelo))
                    return Json(new { exito = false, mensaje = "El modelo es requerido." });
                if (string.IsNullOrWhiteSpace(v.Color))
                    return Json(new { exito = false, mensaje = "El color es requerido." });
                if (string.IsNullOrWhiteSpace(v.Vin) || v.Vin.Length != 17)
                    return Json(new { exito = false, mensaje = "El VIN debe tener exactamente 17 caracteres." });
                if (v.Costo <= 0)
                    return Json(new { exito = false, mensaje = "El costo debe ser mayor a 0." });
                if (_context.Vehiculos.Any(x => x.Vin == v.Vin))
                    return Json(new { exito = false, mensaje = "Ya existe un vehículo registrado con ese VIN." });

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

                return Json(new { exito = true, mensaje = "Vehículo registrado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Ocurrió un error al guardar el vehículo." });
            }
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

            ViewBag.Clientes = _context.Clientes.ToList();
            ViewBag.Estados = _context.Estados.ToList();

            return View("EditarVehiculo", vehiculo);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Edit(VehiculoModel vehiculo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vehiculo.Marca) && string.IsNullOrWhiteSpace(vehiculo.Modelo) &&
                    string.IsNullOrWhiteSpace(vehiculo.Color) && string.IsNullOrWhiteSpace(vehiculo.Vin) &&
                    vehiculo.Costo == 0 && vehiculo.IdCliente == 0)
                    return Json(new { exito = false, mensaje = "Por favor complete el formulario antes de guardar." });

                if (string.IsNullOrWhiteSpace(vehiculo.Marca))
                    return Json(new { exito = false, mensaje = "La marca es requerida." });
                if (string.IsNullOrWhiteSpace(vehiculo.Modelo))
                    return Json(new { exito = false, mensaje = "El modelo es requerido." });
                if (string.IsNullOrWhiteSpace(vehiculo.Color))
                    return Json(new { exito = false, mensaje = "El color es requerido." });
                if (string.IsNullOrWhiteSpace(vehiculo.Vin) || vehiculo.Vin.Length != 17)
                    return Json(new { exito = false, mensaje = "El VIN debe tener exactamente 17 caracteres." });
                if (vehiculo.Costo <= 0)
                    return Json(new { exito = false, mensaje = "El costo debe ser mayor a 0." });


                if (_context.Vehiculos.Any(x => x.Vin == vehiculo.Vin && x.IdVehiculo != vehiculo.IdVehiculo))
                    return Json(new { exito = false, mensaje = "Ya existe un vehículo registrado con ese VIN." });

                _context.Vehiculos.Update(vehiculo);
                _context.SaveChanges();

                return Json(new { exito = true, mensaje = "Vehículo actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Ocurrió un error al actualizar el vehículo." });
            }
        }

        public IActionResult Details(int id)
        {
            var vehiculo = _context.Vehiculos
                .Include(v => v.Estado)
                .Include(v => v.Cliente)
                .FirstOrDefault(v => v.IdVehiculo == id);

            var historial = _context.Historiales
                .Include(h => h.Estado)
                .Where(h => h.IdVehiculo == id)
                .OrderByDescending(h => h.FechaCambio)
                .ToList();

            ViewBag.Estados = _context.Estados.ToList();
            ViewBag.Historial = historial;

            return View("DetallesVehiculo", vehiculo);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult UpdateEstado(int idVehiculo, int idEstado)
        {
            try
            {
                var vehiculo = _context.Vehiculos.Find(idVehiculo);
                if (vehiculo == null)
                    return Json(new { exito = false, mensaje = "Vehículo no encontrado." });

                vehiculo.IdEstado = idEstado;
                _context.Vehiculos.Update(vehiculo);

                var historial = new HistorialEstadoModel
                {
                    IdVehiculo = idVehiculo,
                    IdEstado = idEstado,
                    IdUsuario = 1,
                    FechaCambio = DateTime.Now
                };
                _context.Historiales.Add(historial);
                _context.SaveChanges();

                return Json(new { exito = true, mensaje = "Estado actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Ocurrió un error al actualizar el estado." });
            }
        }

    }
}
