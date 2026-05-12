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
});