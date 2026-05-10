using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Importames.Controllers
{
    public class CotizacionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CalcularPrecioConFees(decimal apuesta)
        {
            decimal multiplicador = 1;

            if (apuesta >= 25 && apuesta < 50)
                multiplicador = 7m;
            else if (apuesta >= 50 && apuesta < 100)
                multiplicador = 5m;
            else if (apuesta >= 100 && apuesta < 150)
                multiplicador = 3.6m;
            else if (apuesta >= 150 && apuesta < 200)
                multiplicador = 2.8m;
            else if (apuesta >= 200 && apuesta < 250)
                multiplicador = 2.6m;
            else if (apuesta >= 250 && apuesta < 300)
                multiplicador = 2.2m;
            else if (apuesta >= 300 && apuesta < 350)
                multiplicador = 2m;
            else if (apuesta >= 350 && apuesta < 500)
                multiplicador = 1.9m;
            else if (apuesta >= 500 && apuesta < 750)
                multiplicador = 1.8m;
            else if (apuesta >= 750 && apuesta < 1000)
                multiplicador = 1.7m;
            else if (apuesta >= 1000 && apuesta < 1250)
                multiplicador = 1.6m;
            else if (apuesta >= 1250 && apuesta < 1500)
                multiplicador = 1.5m;
            else if (apuesta >= 1500 && apuesta < 2000)
                multiplicador = 1.46m;
            else if (apuesta >= 2000 && apuesta < 2500)
                multiplicador = 1.4m;
            else if (apuesta >= 2500 && apuesta < 3000)
                multiplicador = 1.36m;
            else if (apuesta >= 3000 && apuesta < 3500)
                multiplicador = 1.3m;
            else if (apuesta >= 3500 && apuesta < 4000)
                multiplicador = 1.26m;
            else if (apuesta >= 4000 && apuesta < 4500)
                multiplicador = 1.25m;
            else if (apuesta >= 4500 && apuesta < 5000)
                multiplicador = 1.23m;
            else if (apuesta >= 5000 && apuesta < 5500)
                multiplicador = 1.21m;
            else if (apuesta >= 5500 && apuesta < 6500)
                multiplicador = 1.19m;
            else if (apuesta >= 6500 && apuesta < 8000)
                multiplicador = 1.15m;
            else if (apuesta >= 8000 && apuesta < 9500)
                multiplicador = 1.14m;
            else if (apuesta >= 9500 && apuesta < 11000)
                multiplicador = 1.13m;
            else if (apuesta >= 11000 && apuesta < 12000)
                multiplicador = 1.12m;
            else if (apuesta >= 12000 && apuesta < 13000)
                multiplicador = 1.11m;
            else if (apuesta >= 13000 && apuesta < 14000)
                multiplicador = 1.10m;
            else if (apuesta >= 14000 && apuesta < 24000)
                multiplicador = 1.09m;
            else if (apuesta >= 24000 && apuesta < 30000)
                multiplicador = 1.085m;
            else if (apuesta >= 30000 && apuesta < 50000)
                multiplicador = 1.083m;
            else if (apuesta >= 50000 && apuesta < 80000)
                multiplicador = 1.08m;
            else if (apuesta >= 80000 && apuesta < 150000)
                multiplicador = 1.078m;
            else if (apuesta >= 150000)
                multiplicador = 1.076m;

            decimal resultado = apuesta * multiplicador;

            return Json(new
            {
                precioConFees = resultado.ToString("0.00", CultureInfo.InvariantCulture)
            });
        }


        [HttpPost]
        public JsonResult ValidarCampos(List<string> campos)
        {

            foreach (var campo in campos)
            {

                if (string.IsNullOrWhiteSpace(campo))
                {

                    return Json(new
                    {
                        success = false,
                        message = "Todos los campos deben estar llenos"
                    });

                }

            }

            return Json(new
            {
                success = true
            });

        }

    }
}