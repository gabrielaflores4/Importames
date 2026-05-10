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

});

function generarPDF() {

    let campos = document.querySelectorAll("input, select");

    for (let campo of campos) {

        if (campo.type === "hidden") {
            continue;
        }

        if (campo.value == null) {
            continue;
        }

        let valor = campo.value.trim();

        if (valor === "") {

            let nombreCampo = campo.id || "sin nombre";

            alert("El campo está vacío: " + nombreCampo);

            campo.focus();

            return;
        }
    }

    let datos = {

        depositoDeducible:
            document.getElementById("depositoDeducible").value,

        apuestaVehiculo:
            document.getElementById("apuestaVehiculo").value,

        precioFees:
            document.getElementById("precioFees").value,

        comision:
            document.getElementById("comision").value,

        grua:
            document.getElementById("grua").value,

        bl:
            document.getElementById("bl").value,

        flete:
            document.getElementById("flete").value,

        comisionFlete:
            document.getElementById("comisionFlete").value,

        impuesto:
            document.getElementById("impuesto").value,

        tramiteAduanal:
            document.getElementById("tramiteAduanal").value,

        gruaAduana:
            document.getElementById("gruaAduana").value,

        transferencia:
            document.getElementById("transferencia").value,

        storage:
            document.getElementById("storage").value,

        tituloFedex:
            document.getElementById("tituloFedex").value,

        venta:
            document.getElementById("venta").value,

        pagoCuenta:
            document.getElementById("pagoCuenta").value,

        reparacionPintor:
            document.getElementById("reparacionPintor").value,

        repuestos:
            document.getElementById("repuestos").value,

        tramitePlacas:
            document.getElementById("tramitePlacas").value,

        autorizacion:
            document.getElementById("autorizacion").value,

        emisionGases:
            document.getElementById("emisionGases").value,

        experticie:
            document.getElementById("experticie").value,

        experticieSistema:
            document.getElementById("experticieSistema").value,

        nombreCliente:
            document.getElementById("nombreCliente").value,

        telefonoCliente:
            document.getElementById("telefonoCliente").value,

        correoCliente:
            document.getElementById("correoCliente").value,

        marcaVehiculo:
            document.getElementById("marcaVehiculo").value,

        fechaSubasta:
            document.getElementById("fechaSubasta").value,

        subasta:
            document.getElementById("subasta").value,

        numeroStock:
            document.getElementById("numeroStock").value,

        lugarEstado:
            document.getElementById("lugarEstado").value,

        aduanaDestino:
            document.getElementById("aduanaDestino").value,

        naviera:
            document.getElementById("naviera").value
    };

    fetch('/Cotizaciones/GenerarPDF', {

        method: 'POST',

        headers: {
            'Content-Type': 'application/json'
        },

        body: JSON.stringify(datos)

    })
        .then(response => {

            if (!response.ok) {

                return response.text().then(text => {
                    throw new Error(text);
                });
            }

            return response.blob();

        })
        .then(blob => {

            const url = window.URL.createObjectURL(blob);

            const a = document.createElement("a");

            a.href = url;

            a.download = "Cotizacion.pdf";

            document.body.appendChild(a);

            a.click();

            a.remove();

            window.URL.revokeObjectURL(url);

        })
        .catch(error => {

            alert(error.message);

        });

}