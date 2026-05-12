using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Importames.Data;
using Importames.Models;
using System.Linq;
using Importames.Servicios;

namespace Importames.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public class AccountController : Controller
        {
        }

        [Autenticado]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Autenticado]
        [HttpPost]
        public IActionResult Index(string email, string contra)
        {
       
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contra))
            {
                ViewData["Error"] = "Debe ingresar correo y contraseña.";
                return View();
            }

          
            var usuarios = _context.Usuarios
                .FirstOrDefault(u => u.Correo == email && u.Password == contra);

           
            if (usuarios == null)
            {
                ViewData["Error"] = "Credenciales incorrectas. Intente nuevamente.";
                return View();
            }

          
            HttpContext.Session.SetInt32("id_usuarios", usuarios.IdUsuario);
            HttpContext.Session.SetString("correo", usuarios.Correo);
            HttpContext.Session.SetString("nombre_usuario", usuarios.Nombre);
            HttpContext.Session.SetString("rol_usuario", usuarios.Rol);

      
            if (usuarios.Rol == "Administrador")
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            if (usuarios.Rol == "Empleado")
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            return View();
        }

        public IActionResult Logout()      
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}