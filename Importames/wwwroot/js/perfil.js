document.addEventListener("DOMContentLoaded", function () {

    const abrir = document.getElementById("abrirPerfil");

    if (abrir) {

        abrir.addEventListener("click", function (e) {

            e.stopPropagation();

            fetch('/Usuarios/Perfil')
                .then(res => res.text())
                .then(html => {

                    document.getElementById("modalContainer").innerHTML = html;

                    const modal = document.getElementById("perfilModal");
                    const cerrar = document.getElementById("cerrarPerfil");

                    modal.classList.add("active");

                    cerrar.onclick = function () {
                        document.getElementById("modalContainer").innerHTML = "";
                    };

                    modal.onclick = function (e) {

                        if (e.target === modal) {
                            document.getElementById("modalContainer").innerHTML = "";
                        }

                    };

                });

        });

    }

});