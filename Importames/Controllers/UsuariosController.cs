using Importames.Data;
using Importames.Models;
using Importames.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Importames.Controllers
{
    [Autenticado]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // LISTADO
        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            return View("UsuariosView", usuarios);
        }

        // GET: CREAR
        public IActionResult Create()
        {
            return View("CrearUsuario");
        }

        // POST: CREAR
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Create(UsuarioModel u)
        {
            try
            {
                // VALIDAR CAMPOS VACÍOS
                if (string.IsNullOrWhiteSpace(u.Nombre) ||
                    string.IsNullOrWhiteSpace(u.Apellido) ||
                    string.IsNullOrWhiteSpace(u.Username) ||
                    string.IsNullOrWhiteSpace(u.Password) ||
                    string.IsNullOrWhiteSpace(u.Rol) ||
                    string.IsNullOrWhiteSpace(u.Telefono) ||
                    string.IsNullOrWhiteSpace(u.Correo))
                {
                    return Json(new { exito = false, mensaje = "Todos los campos son obligatorios." });
                }

                // VALIDAR TELÉFONO (8 dígitos)
                if (u.Telefono.Length != 8 || !u.Telefono.All(char.IsDigit))
                {
                    return Json(new { exito = false, mensaje = "El teléfono debe tener 8 dígitos numéricos." });
                }

                // VALIDAR USERNAME ÚNICO
                if (_context.Usuarios.Any(x => x.Username == u.Username))
                {
                    return Json(new { exito = false, mensaje = "El username ya existe." });
                }

                _context.Usuarios.Add(u);
                _context.SaveChanges();

                return Json(new { exito = true, mensaje = "Usuario registrado correctamente." });
            }
            catch (Exception)
            {
                return Json(new { exito = false, mensaje = "Error al guardar el usuario." });
            }
        }

        // GET: EDITAR
        public IActionResult Edit(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return NotFound();

            return View("EditarUsuario", usuario);
        }

        // POST: EDITAR
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Edit(UsuarioModel usuario)
        {
            try
            {
                // VALIDAR CAMPOS VACÍOS
                if (string.IsNullOrWhiteSpace(usuario.Nombre) ||
                    string.IsNullOrWhiteSpace(usuario.Apellido) ||
                    string.IsNullOrWhiteSpace(usuario.Username) ||
                    string.IsNullOrWhiteSpace(usuario.Password) ||
                    string.IsNullOrWhiteSpace(usuario.Rol) ||
                    string.IsNullOrWhiteSpace(usuario.Telefono) ||
                    string.IsNullOrWhiteSpace(usuario.Correo))
                {
                    return Json(new { exito = false, mensaje = "Todos los campos son obligatorios." });
                }

                // VALIDAR TELÉFONO
                if (usuario.Telefono.Length != 8 || !usuario.Telefono.All(char.IsDigit))
                {
                    return Json(new { exito = false, mensaje = "El teléfono debe tener 8 dígitos numéricos." });
                }

                // VALIDAR USERNAME ÚNICO (EXCLUYENDO EL ACTUAL)
                if (_context.Usuarios.Any(x => x.Username == usuario.Username && x.IdUsuario != usuario.IdUsuario))
                {
                    return Json(new { exito = false, mensaje = "El username ya pertenece a otro usuario." });
                }

                _context.Usuarios.Update(usuario);
                _context.SaveChanges();

                return Json(new { exito = true, mensaje = "Usuario actualizado correctamente." });
            }
            catch (Exception)
            {
                return Json(new { exito = false, mensaje = "Error al actualizar el usuario." });
            }
        }

        // DETALLES
        public IActionResult Details(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario == null) return NotFound();

            return View("DetallesUsuario", usuario);
        }

        // ELIMINAR
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                var usuario = _context.Usuarios.Find(id);

                if (usuario == null)
                    return Json(new { exito = false, mensaje = "El usuario no existe." });

                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();

                return Json(new { exito = true, mensaje = "Usuario eliminado correctamente." });
            }
            catch (Exception)
            {
                return Json(new { exito = false, mensaje = "Error al eliminar el usuario." });
            }
        }

        public IActionResult Perfil()
        {
            var idUsuario = HttpContext.Session.GetInt32("id_usuario");

            if (idUsuario == null)
            {
                return Unauthorized();
            }

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.IdUsuario == idUsuario);

            if (usuario == null)
            {
                return NotFound();
            }

            return PartialView("PerfilModal", usuario);
        }

    }


}