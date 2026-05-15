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

        if (valor === "") {

            let nombreCampo = campo.id || campo.name || "sin nombre";

            alert(`El campo "${nombreCampo}" está vacío. Por favor complétalo.`);

            campo.focus();

            return;
        }
    }

}