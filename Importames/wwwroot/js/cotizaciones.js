document.addEventListener("DOMContentLoaded", function () {

    document.getElementById("apuestaVehiculo")
        .addEventListener("keyup", function () {

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

    document.getElementById("btnListo")
        .addEventListener("click", function () {

            let formData = new URLSearchParams();

            document.querySelectorAll("input, select")
                .forEach(function (campo, index) {

                    formData.append("campos", campo.value);

                });

            fetch('/Cotizaciones/ValidarCampos', {

                method: 'POST',

                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },

                body: formData

            })
                .then(response => response.json())
                .then(data => {

                    if (!data.success) {

                        alert(data.message);
                        return;
                    }

                    alert("Campos completos");

                    // AQUI DESPUES GENERARAS EL PDF

                });

        });

});