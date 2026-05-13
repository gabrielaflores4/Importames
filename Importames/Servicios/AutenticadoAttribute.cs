using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Importames.Servicios
{
    public class AutenticadoAttribute : ActionFilterAttribute
    {
        private readonly string[] rolesPermitidos;

        public AutenticadoAttribute(params string[] roles)
        {
            rolesPermitidos = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;

            var idUsuario = session.GetInt32("id_usuario");
            var rolUsuario = session.GetString("rol");

            string controller = context.RouteData.Values["controller"]?.ToString();
            string action = context.RouteData.Values["action"]?.ToString();

            if (!controller.Equals("Home", StringComparison.OrdinalIgnoreCase) &&
            !action.Equals("Index", StringComparison.OrdinalIgnoreCase))
            {
                if (!idUsuario.HasValue)
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                    return;
                }

                if (rolesPermitidos.Length > 0)
                {
                    bool autorizado = rolesPermitidos.Contains(rolUsuario);

                    if (!autorizado)
                    {
                        context.Result = new RedirectToActionResult("Dashboard", "Dashboard", null);
                        return;
                    }
                }
            }

            base.OnActionExecuting(context);
        }
    }
}