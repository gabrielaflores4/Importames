function abrirModal() {
    fetch('/Usuarios/Create')
        .then(res => res.text())
        .then(html => {
            document.getElementById("modal-body").innerHTML = html;
            document.getElementById("modal").style.display = "flex";
        })
        .catch(err => alert('Error al intentar abrir el formulario. Verifique su conexión.'));
}

function cerrarModal() {
    document.getElementById("modal").style.display = "none";
}

function abrirModalEditar(id) {
    fetch('/Usuarios/Edit/' + id)
        .then(res => res.text())
        .then(html => {
            document.getElementById("modal-body").innerHTML = html;
            document.getElementById("modal").style.display = "flex";
        })
        .catch(err => alert('Error al intentar abrir el formulario. Verifique su conexión.'));
}

function abrirModalDetalles(id) {
    fetch('/Usuarios/Details/' + id)
        .then(res => res.text())
        .then(html => {
            document.getElementById("modal-body").innerHTML = html;
            document.getElementById("modal").style.display = "flex";
        })
        .catch(err => alert('Error al intentar abrir el formulario. Verifique su conexión.'));
}

function guardarUsuario() {
    const datos = new FormData(document.getElementById('formUsuario'));
    fetch('/Usuarios/Create', { method: 'POST', body: datos })
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
            div.textContent = 'Ha ocurrido un error al guardar el usuario.';
        });
}

function editarUsuario() {
    const datos = new FormData(document.getElementById('formEditarUsuario'));
    fetch('/Usuarios/Edit', { method: 'POST', body: datos })
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
            div.textContent = 'Ha ocurrido un error al editar el usuario.';
        });
}

function eliminarUsuario(id) {
    confirmar('¿Está seguro que desea eliminar este usuario? Esta acción no se puede deshacer.', () => {

        const datos = new FormData();
        datos.append('id', id);

        fetch('/Usuarios/Delete', { method: 'POST', body: datos })
            .then(res => res.json())
            .then(data => {
                if (data.exito) {
                    sessionStorage.setItem('mensaje-exito', data.mensaje);
                    location.reload();
                } else {
                    alert('Atención: ' + data.mensaje);
                }
            })
            .catch(error => {
                console.error("Error en JS:", error);
                alert('Ha ocurrido un error inesperado al procesar la solicitud.');
            });
    });
}

function confirmar(mensaje, callback) {
    document.getElementById('confirm-mensaje').textContent = mensaje;
    document.getElementById('modal-confirm').style.display = 'flex';
    document.getElementById('confirm-aceptar').onclick = () => {
        cancelarConfirm();
        callback();
    };
}

function cancelarConfirm() {
    document.getElementById('modal-confirm').style.display = 'none';
}

window.addEventListener('load', () => {
    const mensaje = sessionStorage.getItem('mensaje-exito');
    if (mensaje) {
        sessionStorage.removeItem('mensaje-exito');
        const div = document.createElement('div');
        div.className = 'alerta alerta-exito';
        div.textContent = mensaje;
        document.querySelector('.table-header').insertAdjacentElement('afterend', div);
        setTimeout(() => {
            div.style.transition = 'opacity 0.5s';
            div.style.opacity = '0';
            setTimeout(() => div.remove(), 500);
        }, 4000);
    }
});