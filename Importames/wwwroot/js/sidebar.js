document.addEventListener("DOMContentLoaded", function () {

    const toggleBtn = document.getElementById("toggleSidebar");
    const sidebar = document.querySelector(".sidebar");
    const mainContent = document.querySelector(".main-content");
    const topbar = document.querySelector(".topbar");

    toggleBtn.addEventListener("click", function (e) {

        e.preventDefault();
        e.stopPropagation();

        sidebar.classList.toggle("closed");
        mainContent.classList.toggle("expanded");
        topbar.classList.toggle("expanded");

    });

});