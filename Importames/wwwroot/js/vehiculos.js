function abrirModal() {
    fetch('/Vehiculos/Create')
        .then(res => res.text())
        .then(html => {
            document.getElementById("modal-body").innerHTML = html;
            document.getElementById("modal").style.display = "flex";
        });
}

function abrirModalEditar(id) {
    fetch('/Vehiculos/Edit/' + id)
        .then(res => res.text())
        .then(html => {
            document.getElementById("modal-body").innerHTML = html;
            document.getElementById("modal").style.display = "flex";
        });
}

function cerrarModal() {
    document.getElementById("modal").style.display = "none";
}

function abrirModalEditar(id) {
    fetch('/Vehiculos/Edit/' + id)
        .then(res => res.text())
        .then(html => {
            document.getElementById("modal-body").innerHTML = html;
            document.getElementById("modal").style.display = "flex";
        });
}

function guardarVehiculo() {
    const datos = new FormData(document.getElementById('formVehiculo'));

    fetch('/Vehiculos/Create', { method: 'POST', body: datos })
        .then(res => res.json())
        .then(data => {
            if (data.exito) {
                cerrarModal();
                sessionStorage.setItem('mensaje-exito', data.mensaje);
                location.reload();
            } else {
                const div = document.getElementById('mensaje-modal');
                div.style.display = 'block';
                div.textContent = 'Mensaje: ' + data.mensaje;
            }
        })
        .catch(() => {
            const div = document.getElementById('mensaje-modal');
            div.style.display = 'block';
            div.textContent = 'Ha ocurrido un error';
        });
}

window.addEventListener('load', () => {
    const mensaje = sessionStorage.getItem('mensaje-exito');
    if (mensaje) {
        sessionStorage.removeItem('mensaje-exito');
        const div = document.createElement('div');
        div.className = 'alerta alerta-exito';
        div.textContent = 'Mensaje: ' + mensaje;
        document.querySelector('.table-header').insertAdjacentElement('afterend', div);
        setTimeout(() => {
            div.style.transition = 'opacity 0.5s';
            div.style.opacity = '0';
            setTimeout(() => div.remove(), 500);
        }, 4000);
    }
});

function editarVehiculo() {
    const datos = new FormData(document.getElementById('formEditar'));

    fetch('/Vehiculos/Edit', { method: 'POST', body: datos })
        .then(res => res.json())
        .then(data => {
            if (data.exito) {
                cerrarModal();
                sessionStorage.setItem('mensaje-exito', data.mensaje);
                location.reload();
            } else {
                const div = document.getElementById('mensaje-modal');
                div.style.display = 'block';
                div.textContent = 'Mensaje: ' + data.mensaje;
            }
        })
        .catch(() => {
            const div = document.getElementById('mensaje-modal');
            div.style.display = 'block';
            div.textContent = 'Ha ocurrido un error';
        });
}