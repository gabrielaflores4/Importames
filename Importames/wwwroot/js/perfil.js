document.addEventListener("DOMContentLoaded", () => {

    document.body.addEventListener("click", async function (e) {

        const btn = e.target.closest("#abrirPerfil");

        if (!btn) return;

        e.preventDefault();
        e.stopImmediatePropagation();

        try {

            const response = await fetch("/Usuarios/Perfil");

            const html = await response.text();

            document.getElementById("modalContainer").innerHTML = html;

            const modal = document.getElementById("perfilModal");

            if (modal) {

                modal.classList.add("active");

                const cerrar = document.getElementById("cerrarPerfil");

                cerrar?.addEventListener("click", () => {

                    modal.classList.remove("active");

                    setTimeout(() => {

                        document.getElementById("modalContainer").innerHTML = "";

                    }, 200);

                });

                modal.addEventListener("click", function (ev) {

                    if (ev.target === modal) {

                        modal.classList.remove("active");

                        setTimeout(() => {

                            document.getElementById("modalContainer").innerHTML = "";

                        }, 200);

                    }

                });

            }

        }
        catch (err) {

            console.log(err);

        }

    });

});