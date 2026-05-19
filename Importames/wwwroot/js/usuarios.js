function abrirModal() {
    fetch('/Usuarios/Create')
        .then(res => res.text())
        .then(html => {
            document.getElementById("modal-body").innerHTML = html;
            document.getElementById("modal").style.display = "flex";
        })
        .catch(() => alert('Error al abrir el formulario.'));
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
        });
}

function abrirModalDetalles(id) {
    fetch('/Usuarios/Details/' + id)
        .then(res => res.text())
        .then(html => {
            document.getElementById("modal-body").innerHTML = html;
            document.getElementById("modal").style.display = "flex";
        });
}

function guardarUsuario() {
    const form = document.getElementById('formUsuario');

    const telefono = form.Telefono.value.trim();
    const correo = form.Correo.value.trim();

    if (!form.Nombre.value.trim() ||
        !form.Apellido.value.trim() ||
        !form.Username.value.trim() ||
        !form.Password.value.trim() ||
        !form.Rol.value ||
        !telefono ||
        !correo) {

        mostrarError("Todos los campos son obligatorios.");
        return;
    }

    if (!/^\d{8}$/.test(telefono)) {
        mostrarError("El teléfono debe tener 8 dígitos.");
        return;
    }

    if (!/\S+@\S+\.\S+/.test(correo)) {
        mostrarError("Correo inválido.");
        return;
    }

    const datos = new FormData(form);

    fetch('/Usuarios/Create', { method: 'POST', body: datos })
        .then(res => res.json())
        .then(data => {
            if (data.exito) {
                cerrarModal();
                sessionStorage.setItem('mensaje-exito', data.mensaje);
                location.reload();
            } else {
                mostrarError(data.mensaje);
            }
        });
}

function editarUsuario() {
    const form = document.getElementById('formEditarUsuario');

    const telefono = form.Telefono.value.trim();
    const correo = form.Correo.value.trim();

    if (!form.Nombre.value.trim() ||
        !form.Apellido.value.trim() ||
        !form.Username.value.trim() ||
        !form.Password.value.trim() ||
        !form.Rol.value ||
        !telefono ||
        !correo) {

        mostrarError("Todos los campos son obligatorios.");
        return;
    }

    if (!/^\d{8}$/.test(telefono)) {
        mostrarError("El teléfono debe tener 8 dígitos.");
        return;
    }

    if (!/\S+@\S+\.\S+/.test(correo)) {
        mostrarError("Correo inválido.");
        return;
    }

    const datos = new FormData(form);

    fetch('/Usuarios/Edit', { method: 'POST', body: datos })
        .then(res => res.json())
        .then(data => {
            if (data.exito) {
                cerrarModal();
                sessionStorage.setItem('mensaje-exito', data.mensaje);
                location.reload();
            } else {
                mostrarError(data.mensaje);
            }
        });
}

function eliminarUsuario(id) {
    confirmar('¿Eliminar este usuario?', () => {

        const datos = new FormData();
        datos.append('id', id);

        fetch('/Usuarios/Delete', { method: 'POST', body: datos })
            .then(res => res.json())
            .then(data => {
                if (data.exito) {
                    sessionStorage.setItem('mensaje-exito', data.mensaje);
                    location.reload();
                } else {
                    alert(data.mensaje);
                }
            });
    });
}

function mostrarError(mensaje) {
    const div = document.getElementById('mensaje-modal');
    if (div) {
        div.style.display = 'block';
        div.textContent = 'Mensaje: ' + mensaje;
    }
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