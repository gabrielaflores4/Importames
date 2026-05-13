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

            return View();
        }
    }
}
