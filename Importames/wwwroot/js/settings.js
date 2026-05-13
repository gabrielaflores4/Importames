document.addEventListener("DOMContentLoaded", function () {

    const openSettings = document.getElementById("openSettings");
    const closeSettings = document.getElementById("closeSettings");
    const settingsModal = document.getElementById("settingsModal");

    if (openSettings) {
        openSettings.addEventListener("click", function () {
            settingsModal.classList.add("active");
        });
    }

    if (closeSettings) {
        closeSettings.addEventListener("click", function () {
            settingsModal.classList.remove("active");
        });
    }

    window.addEventListener("click", function (e) {
        if (e.target === settingsModal) {
            settingsModal.classList.remove("active");
        }
    });

    const darkModeToggle = document.getElementById("darkModeToggle");

    if (localStorage.getItem("darkMode") === "enabled") {

        document.documentElement.classList.add("dark-mode");

        if (darkModeToggle) {
            darkModeToggle.checked = true;
        }
    }

    if (darkModeToggle) {

        darkModeToggle.addEventListener("change", function () {

            if (this.checked) {

                document.documentElement.classList.add("dark-mode");
                localStorage.setItem("darkMode", "enabled");

            } else {

                document.documentElement.classList.remove("dark-mode");
                localStorage.setItem("darkMode", "disabled");

            }

        });

    }

});