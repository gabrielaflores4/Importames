using System.Text;
using Importames.Data;
using Importames.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectPdf;

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
            else
            {
                query = query.Where(v => v.Estado.NombreEstado != "Entregado");
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

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                var vehiculo = _context.Vehiculos
                    .Include(v => v.Historiales)
                    .FirstOrDefault(v => v.IdVehiculo == id);

                if (vehiculo == null)
                    return Json(new { exito = false, mensaje = "Vehículo no encontrado." });

                if (vehiculo.Historiales != null && vehiculo.Historiales.Any())
                {
                    _context.Historiales.RemoveRange(vehiculo.Historiales);
                }

                _context.Vehiculos.Remove(vehiculo);
                _context.SaveChanges();

                return Json(new { exito = true, mensaje = "Vehículo eliminado correctamente." });
            }
            catch (Exception)
            {
                return Json(new { exito = false, mensaje = "Ocurrió un error al eliminar el vehículo." });
            }
        }

        public IActionResult GenerarReporte(int? estadoId)
        {
            var query = _context.Vehiculos
                .Include(v => v.Estado)
                .Include(v => v.Cliente)
                .AsQueryable();

            if (estadoId != null)
            {
                query = query.Where(v => v.IdEstado == estadoId);
            }
            else
            {
                query = query.Where(v => v.Estado.NombreEstado != "Entregado");
            }

            var vehiculos = query.ToList();

            var html = new StringBuilder();

            html.Append(@"
    <html>
    <head>
        <style>

            @page{
                size:A4 landscape;
                margin:20px;
            }

            body{
                font-family: Arial, sans-serif;
                color:#1e293b;
                margin:0;
                padding:0;
            }

            .header{
                display:flex;
                justify-content:space-between;
                align-items:center;
                margin-bottom:30px;
                border-bottom:2px solid #e2e8f0;
                padding-bottom:15px;
            }

            .title{
                font-size:28px;
                font-weight:bold;
                color:#0f172a;
            }

            .subtitle{
                color:#64748b;
                font-size:14px;
                margin-top:5px;
            }

            .date{
                text-align:right;
                color:#64748b;
                font-size:13px;
            }

            .table-wrapper{
                width:100%;
            }

            table{
                width:100%;
                border-collapse:collapse;
                table-layout:fixed;
                margin-top:20px;
            }

            thead{
                display:table-header-group;
                background:#0f172a;
                color:white;
            }

            tbody{
                display:table-row-group;
            }

            tr{
                page-break-inside:avoid !important;
                break-inside:avoid !important;
            }

            th{
                padding:14px;
                font-size:13px;
                text-align:center;
                word-wrap:break-word;
            }

            td{
                padding:14px;
                border-bottom:1px solid #e2e8f0;
                font-size:13px;
                text-align:center;
                vertical-align:middle;
                word-wrap:break-word;
            }

            tbody tr:nth-child(even){
                background:#f8fafc;
            }

            .badge{
                padding:6px 12px;
                border-radius:20px;
                font-size:12px;
                font-weight:bold;
                color:white;
                display:inline-block;
            }

            .cyan{ background:#06b6d4; }
            .yellow{ background:#eab308; }
            .purple{ background:#8b5cf6; }
            .red{ background:#ef4444; }
            .blue{ background:#3b82f6; }
            .green{ background:#22c55e; }

            .footer{
                margin-top:30px;
                text-align:center;
                color:#94a3b8;
                font-size:12px;
            }

        </style>
    </head>

    <body>

        <div class='header'>
            <div>
                <div class='title'>Reporte de Vehículos</div>

                <div class='subtitle'>
                    Vehículos filtrados del sistema
                </div>
            </div>

            <div class='date'>
                Fecha de generación<br/>
                " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + @"
            </div>
        </div>

        <div class='table-wrapper'>

            <table>

                <thead>
                    <tr>
                        <th>VIN</th>
                        <th>Vehículo</th>
                        <th>Cliente</th>
                        <th>Estado</th>
                        <th>Año</th>
                        <th>Costo</th>
                    </tr>
                </thead>

                <tbody>
    ");

            foreach (var v in vehiculos)
            {
                string clase = "blue";

                switch (v.Estado.NombreEstado)
                {
                    case "En tránsito":
                        clase = "cyan";
                        break;

                    case "En aduana":
                        clase = "yellow";
                        break;

                    case "En preparación":
                        clase = "purple";
                        break;

                    case "Trámites pendientes":
                        clase = "red";
                        break;

                    case "Listo para entrega":
                        clase = "blue";
                        break;

                    case "Entregado":
                        clase = "green";
                        break;
                }

                html.Append($@"
            <tr>
                <td>{v.Vin}</td>

                <td>
                    {v.Marca} {v.Modelo}
                </td>

                <td>
                    {v.Cliente.Nombre} {v.Cliente.Apellido}
                </td>

                <td>
                    <span class='badge {clase}'>
                        {v.Estado.NombreEstado}
                    </span>
                </td>

                <td>
                    {v.Anio}
                </td>

                <td>
                    ${v.Costo}
                </td>
            </tr>
        ");
            }

            html.Append(@"
                </tbody>

            </table>

        </div>

        <div class='footer'>
            Importames © " + DateTime.Now.Year + @"
        </div>

    </body>
    </html>
    ");

            HtmlToPdf converter = new HtmlToPdf();

            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;

            converter.Options.MarginTop = 20;
            converter.Options.MarginBottom = 20;
            converter.Options.MarginLeft = 20;
            converter.Options.MarginRight = 20;

            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.NoAdjustment;
            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;

            PdfDocument doc = converter.ConvertHtmlString(html.ToString());

            byte[] pdf = doc.Save();

            doc.Close();

            return File(pdf, "application/pdf", "ReporteVehiculos.pdf");
        }
    }
}
