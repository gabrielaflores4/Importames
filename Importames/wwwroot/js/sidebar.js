document.addEventListener("DOMContentLoaded", function () {
    const sidebar = document.querySelector(".sidebar");
    const mainContent = document.querySelector(".main-content");
    const topbar = document.querySelector(".topbar");
    const toggleBtn = document.getElementById("toggleSidebar");

    sidebar.style.transition = "none";
    mainContent.style.transition = "none";
    topbar.style.transition = "none";

    requestAnimationFrame(() => {
        setTimeout(() => {
            sidebar.style.transition = "";
            mainContent.style.transition = "";
            topbar.style.transition = "";
        }, 50);
    });

    toggleBtn.addEventListener("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        sidebar.classList.toggle("closed");
        mainContent.classList.toggle("expanded");
        topbar.classList.toggle("expanded");
        const isClosed = sidebar.classList.contains("closed");
        document.cookie = `sidebarClosed=${isClosed};path=/;max-age=31536000`;
    });

    const userMenuBtn = document.getElementById("userMenuBtn");
    const userDropdown = document.getElementById("userDropdown");

    userMenuBtn.addEventListener("click", function (e) {
        e.stopPropagation();
        userDropdown.classList.toggle("open");
    });

    document.addEventListener("click", function () {
        userDropdown.classList.remove("open");
    });

    document.addEventListener("click", function (e) {

        const link = e.target.closest("#abrirPerfil");

        if (link) {

            e.preventDefault();

            fetch("/Usuarios/Perfil")
                .then(r => r.text())
                .then(html => {

                    document.getElementById("modalContainer").innerHTML = html;

                });

        }

        if (e.target.closest("#cerrarPerfil")) {
            document.getElementById("modalContainer").innerHTML = "";
        }

    });

});