document.addEventListener("DOMContentLoaded", function () {

    const apuestaVehiculo = document.getElementById("apuestaVehiculo");

    apuestaVehiculo.addEventListener("keyup", function () {

        let apuesta = this.value;

        if (apuesta === "") {

            document.getElementById("precioFees").value = "";
            return;
        }

        fetch('/Cotizaciones/CalcularPrecioConFees', {

            method: 'POST',

            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },

            body: 'apuesta=' + apuesta

        })
            .then(response => response.json())
            .then(data => {

                document.getElementById("precioFees").value =
                    data.precioConFees;

            });

    });




    document.getElementById("buscarCliente")
        .addEventListener("click", function () {

            const dui = document.getElementById("duiCliente").value;

            if (dui.trim() === "") {
                alert("Ingrese un DUI");
                return;
            }

            fetch(`/Clientes/BuscarPorDui?dui=${dui}`)
                .then(res => res.json())
                .then(data => {

                    if (data.encontrado) {

                        document.getElementById("nombreCliente").value = data.nombre;
                        document.getElementById("telefonoCliente").value = data.telefono;
                        document.getElementById("correoCliente").value = data.correo;

                    } else {

                        const registrar = confirm(
                            "Cliente no encontrado. ¿Desea registrarlo?"
                        );

                        if (registrar) {

                            document.getElementById("nombreCliente").focus();

                        }

                    }

                });

        });

});

function generarPDF() {

    let campos = document.querySelectorAll("input, select");

    const camposNumericos = [
        "apuestaVehiculo", "precioFees", "comision", "grua", "bl", "flete",
        "comisionFlete", "impuesto", "tramiteAduanal", "gruaAduana", "transferencia",
        "storage", "tituloFedex", "venta", "pagoCuenta", "depositoDeducible",
        "reparacionPintor", "repuestos", "tramitePlacas", "autorizacion",
        "emisionGases", "experticie", "experticieSistema"
    ];

    for (let campo of campos) {

        if (campo.type === "hidden") continue;

        let valor = campo.value ? campo.value.trim() : "";

        if (valor === "" && camposNumericos.includes(campo.id)) {
            campo.value = "0";
            valor = "0";
        }
    }

    let d = {};
    for (let campo of campos) {
        if (campo.type !== "hidden" && campo.id) {
            d[campo.id] = campo.value.trim();
        }
    }

    // Convertir a numéricos
    const getNum = (id) => parseFloat(d[id]) || 0;

    let precioFees = getNum("precioFees");
    let comision = getNum("comision");
    let transferencia = getNum("transferencia");
    let storage = getNum("storage");
    let tituloFedex = getNum("tituloFedex");
    let grua = getNum("grua");
    let depositoDeducible = getNum("depositoDeducible");
    let bl = getNum("bl");
    let flete = getNum("flete");
    let comisionFlete = getNum("comisionFlete");
    
    let fleteIBK = bl + flete + comisionFlete;

    let impuesto = getNum("impuesto");
    let tramiteAduanal = getNum("tramiteAduanal");
    let gruaAduana = getNum("gruaAduana");
    let venta = getNum("venta");
    let pagoCuenta = getNum("pagoCuenta");

    let reparacionPintor = getNum("reparacionPintor");
    let repuestos = getNum("repuestos");
    let tramitePlacas = getNum("tramitePlacas");
    let autorizacion = getNum("autorizacion");
    let emisionGases = getNum("emisionGases");
    let experticie = getNum("experticie");
    let experticieSistema = getNum("experticieSistema");

    let total = precioFees + comision + transferencia + storage + tituloFedex + grua +
                fleteIBK + impuesto + tramiteAduanal + gruaAduana + venta + pagoCuenta +
                reparacionPintor + repuestos + tramitePlacas + autorizacion + emisionGases +
                experticie + experticieSistema;

    let subtotalDerecha = precioFees + comision + transferencia + storage + grua + tituloFedex;
    let totalDerecha = subtotalDerecha - depositoDeducible;

    const fmt = (num) => num.toFixed(2);

    // Plantilla HTML del PDF, idéntica a la que generaba el servidor
    let html = `
    <html>
    <head>
        <title>Cotizacion - ${d["nombreCliente"] || "Vehiculo"}</title>
        <style>
            body { font-family: Arial, sans-serif; padding: 20px; }
            table { border-collapse: collapse; width: 100%; }
            td, th { border: 1px solid black; padding: 6px; font-size: 12px; }
            .titulo { text-align: center; font-size: 38px; font-weight: bold; margin-bottom: 20px; }
            .rojo { color: #c40000; font-weight: bold; }
            .amarillo { background: #fff3a1 !important; -webkit-print-color-adjust: exact; print-color-adjust: exact; }
            .centrado { text-align: center; }
            .derecha { text-align: right; }
            .gris { background: #f2f2f2 !important; -webkit-print-color-adjust: exact; print-color-adjust: exact; }
            
            @media print {
                @page { margin: 0; } /* Elimina encabezados y pies de página (IP, Fecha) */
                html, body { 
                    margin: 0; 
                    padding: 0; 
                    width: 100%; 
                    height: 100%; 
                    overflow: hidden; /* Esto es LA MAGIA para evitar la segunda página en blanco */
                }
                .contenedor {
                    padding: 1.5cm; /* Añade el margen al contenido para compensar */
                    box-sizing: border-box;
                }
                .btn-imprimir { display: none; }
            }
        </style>
    </head>
    <body>
        <div class="contenedor">
            <div class="btn-imprimir" style="text-align: right; margin-bottom: 10px;">
                <button onclick="window.print()" style="padding: 10px 20px; font-size: 16px; cursor: pointer; background: #007bff; color: white; border: none; border-radius: 5px;">🖨️ Guardar PDF / Imprimir</button>
            </div>
            <div class='titulo'>COTIZACION</div>
        <table>
            <tr>
                <td style='width:65%; vertical-align:top;'>
                    <table>
                        <tr><td colspan='2' class='rojo centrado'>ESTIMADO DE VEHICULO</td></tr>
                        <tr><td><b>NOMBRE CLIENTE</b></td><td>${d["nombreCliente"] || ""}</td></tr>
                        <tr><td><b>TELEFONO</b></td><td>${d["telefonoCliente"] || ""}</td></tr>
                        <tr><td><b>CORREO</b></td><td>${d["correoCliente"] || ""}</td></tr>
                        <tr><td><b>MARCA VEHICULO</b></td><td>${d["marcaVehiculo"] || ""}</td></tr>
                        <tr><td><b>FECHA SUBASTA</b></td><td>${d["fechaSubasta"] || ""}</td></tr>
                        <tr><td><b>SUBASTA</b></td><td>${d["subasta"] || ""}</td></tr>
                        <tr><td><b>NUMERO STOCK</b></td><td>${d["numeroStock"] || ""}</td></tr>
                        <tr><td><b>LUGAR / ESTADO</b></td><td>${d["lugarEstado"] || ""}</td></tr>
                        <tr><td><b>ADUANA DESTINO</b></td><td>${d["aduanaDestino"] || ""}</td></tr>
                        <tr><td><b>COMPRA DE AUTO</b></td><td class='amarillo'>$ ${d["apuestaVehiculo"] || "0"} + FEES $ ${fmt(precioFees)}</td></tr>
                        <tr><td><b>COMISION</b></td><td>$ ${fmt(comision)}</td></tr>
                        <tr><td><b>TRANSFERENCIA</b></td><td>$ ${fmt(transferencia)}</td></tr>
                        <tr><td><b>STORAGE</b></td><td>$ ${fmt(storage)}</td></tr>
                        <tr><td><b>TITULO FEDEX</b></td><td>$ ${fmt(tituloFedex)}</td></tr>
                        <tr><td><b>GRUA USA</b></td><td>$ ${fmt(grua)}</td></tr>
                        <tr><td class='rojo'><b>FLETE IBK</b></td><td>$ ${fmt(fleteIBK)}</td></tr>
                        <tr><td><b>IMPUESTO ADUANAL</b></td><td>$ ${fmt(impuesto)}</td></tr>
                        <tr><td><b>TRAMITE ADUANAL</b></td><td>$ ${fmt(tramiteAduanal)}</td></tr>
                        <tr><td><b>GRUA DE ADUANA</b></td><td>$ ${fmt(gruaAduana)}</td></tr>
                        <tr><td><b>VENTA</b></td><td>$ ${fmt(venta)}</td></tr>
                        <tr><td><b>PAGO A CUENTA</b></td><td>$ ${fmt(pagoCuenta)}</td></tr>
                        <tr><td><b>REPARACION PINTOR</b></td><td>$ ${fmt(reparacionPintor)}</td></tr>
                        <tr><td><b>REPUESTOS</b></td><td>$ ${fmt(repuestos)}</td></tr>
                        <tr><td><b>TRAMITE PLACAS</b></td><td>$ ${fmt(tramitePlacas)}</td></tr>
                        <tr><td><b>AUTORIZACION</b></td><td>$ ${fmt(autorizacion)}</td></tr>
                        <tr><td><b>EMISION GASES</b></td><td>$ ${fmt(emisionGases)}</td></tr>
                        <tr><td><b>EXPERTICIE</b></td><td>$ ${fmt(experticie)}</td></tr>
                        <tr><td><b>EXPERTICIE SISTEMA</b></td><td>$ ${fmt(experticieSistema)}</td></tr>
                        <tr><td colspan='2' class='amarillo derecha'><b>TOTAL: $ ${fmt(total)}</b></td></tr>
                    </table>
                </td>
                <td style='width:35%; vertical-align:top;'>
                    <table>
                        <tr><td class='rojo'><b>COMPRA VEHICULO</b></td><td>$ ${fmt(precioFees)}</td></tr>
                        <tr><td class='rojo'><b>COMISION</b></td><td>$ ${fmt(comision)}</td></tr>
                        <tr><td class='rojo'><b>TRANSFERENCIA</b></td><td>$ ${fmt(transferencia)}</td></tr>
                        <tr><td class='rojo'><b>STORAGE</b></td><td>$ ${fmt(storage)}</td></tr>
                        <tr><td class='rojo'><b>GRUA USA</b></td><td>$ ${fmt(grua)}</td></tr>
                        <tr><td class='rojo'><b>TITULO FEDEX</b></td><td>$ ${fmt(tituloFedex)}</td></tr>
                        <tr><td class='rojo'><b>SUB TOTAL</b></td><td class='amarillo'>$ ${fmt(subtotalDerecha)}</td></tr>
                        <tr><td class='rojo'><b>DEPOSITO DEDUCIBLE</b></td><td>$ ${fmt(depositoDeducible)}</td></tr>
                        <tr><td class='rojo'><b>TOTAL</b></td><td class='amarillo'>$ ${fmt(totalDerecha)}</td></tr>
                    </table>
                </td>
            </tr>
        </table>
        <br><br>
        <div style='text-align:center; font-weight:bold; font-size:13px;'>
            Banco Agricola Cuenta # 003760364703<br>
            A nombre de: Carlos Roberto Amaya<br>
            Por favor anexar hojas verdes y derecho de deposito. Gracias!
        </div>
        </div>
        <script>
            // Disparar la impresión automáticamente al cargar la ventana
            window.onload = function() {
                setTimeout(function() { window.print(); }, 500);
            };
        </script>
    </body>
    </html>`;

    // Abre una nueva pestaña y escribe el HTML
    let pdfWindow = window.open("", "_blank");
    if (!pdfWindow) {
        alert("El navegador bloqueó la ventana emergente. Por favor, permite los pop-ups para este sitio.");
        return;
    }
    
    pdfWindow.document.open();
    pdfWindow.document.write(html);
    pdfWindow.document.close();

}