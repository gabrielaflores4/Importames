using Importames.Data;
using Microsoft.AspNetCore.Mvc;
using Importames.Servicios;

namespace Importames.Controllers
{
    [Autenticado]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            // Vehículos que NO están entregados
            ViewBag.VehiculosPendientes = _context.Vehiculos
                .Where(v => v.Estado.NombreEstado != "Entregado")
                .Count();

            // Vehículos entregados
            ViewBag.Entregados = _context.Vehiculos
                .Where(v => v.Estado.NombreEstado == "Entregado")
                .Count();

            // Total de clientes
            ViewBag.Clientes = _context.Clientes.Count();

            // Total de empleados
            ViewBag.Empleados = _context.Usuarios.Count();

            // Total inventario
            ViewBag.TotalInventario = _context.Vehiculos
                .Sum(v => v.Costo);

            // MARCA MÁS REPETIDA
            var marcaMasComprada = _context.Vehiculos
                .GroupBy(v => v.Marca)
                .Select(g => new
                {
                    Marca = g.Key,
                    Cantidad = g.Count()
                })
                .OrderByDescending(x => x.Cantidad)
                .FirstOrDefault();

            if (marcaMasComprada != null)
            {
                ViewBag.MarcaMasComprada = marcaMasComprada.Marca;
                ViewBag.CantidadMarca = marcaMasComprada.Cantidad;
            }
            else
            {
                ViewBag.MarcaMasComprada = "-";
                ViewBag.CantidadMarca = 0;
            }

            return View();
        }
    }
}
