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

    let btnListo = document.querySelector('.btn-pdf');
    let textoOriginal = "LISTO!";
    if (btnListo) {
        textoOriginal = btnListo.innerHTML;
        btnListo.innerHTML = "Generando PDF... (Esto puede tardar un poco)";
        btnListo.disabled = true;
        btnListo.style.opacity = "0.7";
    }

    let datos = {};
    for (let campo of campos) {
        if (campo.type !== "hidden" && campo.id) {
            datos[campo.id] = campo.value.trim();
        }
    }

    fetch('/Cotizaciones/GenerarPDF', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(datos)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error al generar el PDF. Verifica los datos.');
        }
        return response.blob();
    })
    .then(blob => {
        const fileURL = window.URL.createObjectURL(blob);
        
        // Crea un elemento <a> para forzar la descarga y luego abre
        // Esto soluciona ambos problemas: se descarga y se abre
        const a = document.createElement('a');
        a.style.display = 'none';
        a.href = fileURL;
        a.download = 'Cotizacion.pdf';
        
        // Requerido por Firefox
        document.body.appendChild(a);
        a.click();
        
        // Además, lo abre en una nueva pestaña
        window.open(fileURL, '_blank');
        
        document.body.removeChild(a);
        
        // Restaurar botón
        if (btnListo) {
            btnListo.innerHTML = textoOriginal;
            btnListo.disabled = false;
            btnListo.style.opacity = "1";
        }

        setTimeout(() => {
            window.URL.revokeObjectURL(fileURL);
        }, 8000);
    })
    .catch(error => {
        alert(error.message);
        // Restaurar botón
        if (btnListo) {
            btnListo.innerHTML = textoOriginal;
            btnListo.disabled = false;
            btnListo.style.opacity = "1";
        }
    });

}