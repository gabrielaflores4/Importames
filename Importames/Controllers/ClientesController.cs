using Importames.Data;
using Importames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Importames.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // Listado de clientes
        public IActionResult Index()
        {
            // Obtenemos la lista de clientes desde el context
            var clientes = _context.Clientes.ToList();
            return View("ClientesView", clientes);
        }

        // GET: Mostrar formulario de creación
        public IActionResult Create()
        {
            return View("CrearCliente");
        }

        // POST: Guardar nuevo cliente
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Create(ClienteModel c)
        {
            try
            {
                // Validaciones básicas según tu SQL
                if (string.IsNullOrWhiteSpace(c.Nombre) || string.IsNullOrWhiteSpace(c.Apellido))
                    return Json(new { exito = false, mensaje = "El nombre y apellido son obligatorios." });

                if (string.IsNullOrWhiteSpace(c.Dui))
                    return Json(new { exito = false, mensaje = "El DUI es requerido." });

                // Verificar si ya existe el DUI
                if (_context.Clientes.Any(x => x.Dui == c.Dui))
                    return Json(new { exito = false, mensaje = "Ya existe un cliente registrado con este DUI." });

                c.FechaRegistro = DateTime.Now;
                
                _context.Clientes.Add(c);
                _context.SaveChanges();

                return Json(new { exito = true, mensaje = "Cliente registrado correctamente." });
            }
            catch (Exception)
            {
                return Json(new { exito = false, mensaje = "Ocurrió un error al guardar el cliente." });
            }
        }

        // GET: Mostrar formulario de edición
        public IActionResult Edit(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return NotFound();

            return View("EditarCliente", cliente);
        }

        // POST: Actualizar cliente
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Edit(ClienteModel cliente)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cliente.Nombre) || string.IsNullOrWhiteSpace(cliente.Apellido))
                    return Json(new { exito = false, mensaje = "Campos obligatorios vacíos." });

                // Validar DUI duplicado omitiendo al cliente actual
                if (_context.Clientes.Any(x => x.Dui == cliente.Dui && x.IdCliente != cliente.IdCliente))
                    return Json(new { exito = false, mensaje = "Este DUI ya pertenece a otro cliente." });

                _context.Clientes.Update(cliente);
                _context.SaveChanges();

                return Json(new { exito = true, mensaje = "Cliente actualizado correctamente." });
            }
            catch (Exception)
            {
                return Json(new { exito = false, mensaje = "Error al actualizar los datos." });
            }
        }

        // GET: Detalles del cliente
        public IActionResult Details(int id)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.IdCliente == id);
            
            if (cliente == null) return NotFound();

            // Retornamos la vista correcta
            return View("DetallesCliente", cliente);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                var cliente = _context.Clientes.Find(id);
                
                if (cliente == null)
                {
                    return Json(new { exito = false, mensaje = "El cliente no existe." });
                }

                // Validación de Integridad Referencial (¡MUY IMPORTANTE!)
                // Revisamos si el cliente tiene vehículos asociados
                bool tieneVehiculos = _context.Vehiculos.Any(v => v.IdCliente == id);
                if (tieneVehiculos)
                {
                    return Json(new { 
                        exito = false, 
                        mensaje = "No se puede eliminar este cliente porque tiene vehículos registrados a su nombre. Elimine o reasigne los vehículos primero." 
                    });
                }

                _context.Clientes.Remove(cliente);
                _context.SaveChanges();

                return Json(new { exito = true, mensaje = "Cliente eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Ocurrió un error al intentar eliminar el cliente." });
            }
        }
    }
}