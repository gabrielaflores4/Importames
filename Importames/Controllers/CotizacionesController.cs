using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System.Text.Json;

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
        public IActionResult GenerarPDF([FromBody] JsonElement datos)
        {
            foreach (JsonProperty campo in datos.EnumerateObject())
            {
                if (string.IsNullOrWhiteSpace(campo.Value.ToString()))
                {
                    return BadRequest("Todos los campos deben estar llenos");
                }
            }

            decimal ObtenerDecimal(string propiedad)
            {
                string valor = datos.GetProperty(propiedad).ToString();

                if (string.IsNullOrWhiteSpace(valor))
                    return 0;

                return Convert.ToDecimal(valor, CultureInfo.InvariantCulture);
            }

            decimal precioFees = ObtenerDecimal("precioFees");
            decimal comision = ObtenerDecimal("comision");
            decimal transferencia = ObtenerDecimal("transferencia");
            decimal storage = ObtenerDecimal("storage");
            decimal tituloFedex = ObtenerDecimal("tituloFedex");
            decimal grua = ObtenerDecimal("grua");
            decimal depositoDeducible = ObtenerDecimal("depositoDeducible");
            decimal bl = ObtenerDecimal("bl");
            decimal flete = ObtenerDecimal("flete");
            decimal comisionFlete = ObtenerDecimal("comisionFlete");

            decimal fleteIBK = bl + flete + comisionFlete;

            decimal impuesto = ObtenerDecimal("impuesto");
            decimal tramiteAduanal = ObtenerDecimal("tramiteAduanal");
            decimal gruaAduana = ObtenerDecimal("gruaAduana");
            decimal venta = ObtenerDecimal("venta");
            decimal pagoCuenta = ObtenerDecimal("pagoCuenta");

            decimal reparacionPintor = ObtenerDecimal("reparacionPintor");
            decimal repuestos = ObtenerDecimal("repuestos");
            decimal tramitePlacas = ObtenerDecimal("tramitePlacas");
            decimal autorizacion = ObtenerDecimal("autorizacion");
            decimal emisionGases = ObtenerDecimal("emisionGases");
            decimal experticie = ObtenerDecimal("experticie");
            decimal experticieSistema = ObtenerDecimal("experticieSistema");

            decimal total =
                precioFees +
                comision +
                transferencia +
                storage +
                tituloFedex +
                grua +
                fleteIBK +
                impuesto +
                tramiteAduanal +
                gruaAduana +
                venta +
                pagoCuenta +
                reparacionPintor +
                repuestos +
                tramitePlacas +
                autorizacion +
                emisionGases +
                experticie +
                experticieSistema;

            decimal subtotalDerecha =
                precioFees +
                comision +
                transferencia +
                storage +
                grua +
                tituloFedex;

            decimal totalDerecha = subtotalDerecha - depositoDeducible;

            string html = $@"
<html>

<head>

<style>

body {{
    font-family: Arial;
    padding:20px;
}}

table {{
    border-collapse: collapse;
    width: 100%;
}}

td, th {{
    border: 1px solid black;
    padding: 6px;
    font-size: 12px;
}}

.titulo {{
    text-align:center;
    font-size:38px;
    font-weight:bold;
    margin-bottom:20px;
}}

.rojo {{
    color:#c40000;
    font-weight:bold;
}}

.amarillo {{
    background:#fff3a1;
}}

.centrado {{
    text-align:center;
}}

.derecha {{
    text-align:right;
}}

.gris {{
    background:#f2f2f2;
}}

</style>

</head>

<body>

<div class='titulo'>
    COTIZACION
</div>

<table>

<tr>

<td style='width:65%; vertical-align:top;'>

<table>

<tr>
    <td colspan='2' class='rojo centrado'>
        ESTIMADO DE VEHICULO
    </td>
</tr>

<tr>
    <td><b>NOMBRE CLIENTE</b></td>
    <td>{datos.GetProperty("nombreCliente")}</td>
</tr>

<tr>
    <td><b>TELEFONO</b></td>
    <td>{datos.GetProperty("telefonoCliente")}</td>
</tr>

<tr>
    <td><b>CORREO</b></td>
    <td>{datos.GetProperty("correoCliente")}</td>
</tr>

<tr>
    <td><b>MARCA VEHICULO</b></td>
    <td>{datos.GetProperty("marcaVehiculo")}</td>
</tr>

<tr>
    <td><b>FECHA SUBASTA</b></td>
    <td>{datos.GetProperty("fechaSubasta")}</td>
</tr>

<tr>
    <td><b>SUBASTA</b></td>
    <td>{datos.GetProperty("subasta")}</td>
</tr>

<tr>
    <td><b>NUMERO STOCK</b></td>
    <td>{datos.GetProperty("numeroStock")}</td>
</tr>

<tr>
    <td><b>LUGAR / ESTADO</b></td>
    <td>{datos.GetProperty("lugarEstado")}</td>
</tr>

<tr>
    <td><b>ADUANA DESTINO</b></td>
    <td>{datos.GetProperty("aduanaDestino")}</td>
</tr>

<tr>
    <td><b>COMPRA DE AUTO</b></td>

    <td class='amarillo'>
        $
        {datos.GetProperty("apuestaVehiculo")}
        +
        FEES
        $
        {precioFees}
    </td>
</tr>

<tr>
    <td><b>COMISION</b></td>
    <td>$ {comision}</td>
</tr>

<tr>
    <td><b>TRANSFERENCIA</b></td>
    <td>$ {transferencia}</td>
</tr>

<tr>
    <td><b>STORAGE</b></td>
    <td>$ {storage}</td>
</tr>

<tr>
    <td><b>TITULO FEDEX</b></td>
    <td>$ {tituloFedex}</td>
</tr>

<tr>
    <td><b>GRUA USA</b></td>
    <td>$ {grua}</td>
</tr>

<tr>
    <td class='rojo'><b>FLETE IBK</b></td>
    <td>$ {fleteIBK}</td>
</tr>

<tr>
    <td><b>IMPUESTO ADUANAL</b></td>
    <td>$ {impuesto}</td>
</tr>

<tr>
    <td><b>TRAMITE ADUANAL</b></td>
    <td>$ {tramiteAduanal}</td>
</tr>

<tr>
    <td><b>GRUA DE ADUANA</b></td>
    <td>$ {gruaAduana}</td>
</tr>

<tr>
    <td><b>VENTA</b></td>
    <td>$ {venta}</td>
</tr>

<tr>
    <td><b>PAGO A CUENTA</b></td>
    <td>$ {pagoCuenta}</td>
</tr>

<tr>
    <td><b>REPARACION PINTOR</b></td>
    <td>$ {reparacionPintor}</td>
</tr>

<tr>
    <td><b>REPUESTOS</b></td>
    <td>$ {repuestos}</td>
</tr>

<tr>
    <td><b>TRAMITE PLACAS</b></td>
    <td>$ {tramitePlacas}</td>
</tr>

<tr>
    <td><b>AUTORIZACION</b></td>
    <td>$ {autorizacion}</td>
</tr>

<tr>
    <td><b>EMISION GASES</b></td>
    <td>$ {emisionGases}</td>
</tr>

<tr>
    <td><b>EXPERTICIE</b></td>
    <td>$ {experticie}</td>
</tr>

<tr>
    <td><b>EXPERTICIE SISTEMA</b></td>
    <td>$ {experticieSistema}</td>
</tr>

<tr>
    <td colspan='2' class='amarillo derecha'>
        <b>TOTAL: $ {total}</b>
    </td>
</tr>

</table>

</td>

<td style='width:35%; vertical-align:top;'>

<table>

<tr>
    <td class='rojo'><b>COMPRA VEHICULO</b></td>
    <td>$ {precioFees}</td>
</tr>

<tr>
    <td class='rojo'><b>COMISION</b></td>
    <td>$ {comision}</td>
</tr>

<tr>
    <td class='rojo'><b>TRANSFERENCIA</b></td>
    <td>$ {transferencia}</td>
</tr>

<tr>
    <td class='rojo'><b>STORAGE</b></td>
    <td>$ {storage}</td>
</tr>

<tr>
    <td class='rojo'><b>GRUA USA</b></td>
    <td>$ {grua}</td>
</tr>

<tr>
    <td class='rojo'><b>TITULO FEDEX</b></td>
    <td>$ {tituloFedex}</td>
</tr>

<tr>
    <td class='rojo'><b>SUB TOTAL</b></td>
    <td class='amarillo'>
        $ {subtotalDerecha}
    </td>
</tr>

<tr>
    <td class='rojo'><b>DEPOSITO DEDUCIBLE</b></td>
    <td>
        $ {depositoDeducible}
    </td>
</tr>

<tr>
    <td class='rojo'><b>TOTAL</b></td>
    <td class='amarillo'>
        $ {totalDerecha}
    </td>
</tr>

</table>

</td>

</tr>

</table>

<br><br>

<div style='text-align:center; font-weight:bold; font-size:13px;'>

Banco Agricola Cuenta # 003760364703
<br>
A nombre de: Carlos Roberto Amaya
<br>
Por favor anexar hojas verdes y derecho de deposito. Gracias!

</div>

</body>
</html>";

            HtmlToPdf converter = new HtmlToPdf();

            PdfDocument doc = converter.ConvertHtmlString(html);

            byte[] pdf = doc.Save();

            doc.Close();

            return File(pdf, "application/pdf", "Cotizacion.pdf");
        }

        [HttpPost]
        public JsonResult ValidarCampos(List<string> campos)
        {
            foreach (var campo in campos)
            {
                if (campo == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Todos los campos deben estar llenos"
                    });
                }

                if (string.IsNullOrWhiteSpace(campo.Trim()))
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