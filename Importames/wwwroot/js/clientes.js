function abrirModal() {
    fetch('/Clientes/Create')
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
    fetch('/Clientes/Edit/' + id)
        .then(res => res.text())
        .then(html => {
            document.getElementById("modal-body").innerHTML = html;
            document.getElementById("modal").style.display = "flex";
        })
        .catch(err => alert('Error al intentar abrir el formulario. Verifique su conexión.'));
}

function abrirModalDetalles(id) {
    fetch('/Clientes/Details/' + id)
        .then(res => res.text())
        .then(html => {
            document.getElementById("modal-body").innerHTML = html;
            document.getElementById("modal").style.display = "flex";
        })
        .catch(err => alert('Error al intentar abrir el formulario. Verifique su conexión.'));
}

function guardarCliente() {
    const datos = new FormData(document.getElementById('formCliente'));
    fetch('/Clientes/Create', { method: 'POST', body: datos })
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
            div.textContent = 'Ha ocurrido un error al guardar el cliente.';
        });
}

function editarCliente() {
    const datos = new FormData(document.getElementById('formEditarCliente'));
    fetch('/Clientes/Edit', { method: 'POST', body: datos })
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
            div.textContent = 'Ha ocurrido un error al editar el cliente.';
        });
}

function eliminarCliente(id) {
    confirmar('¿Está seguro que desea eliminar este cliente? Esta acción no se puede deshacer.', () => {
        
        const datos = new FormData();
        datos.append('id', id);

        fetch('/Clientes/Delete', { method: 'POST', body: datos })
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

    document.addEventListener("input", function (e) {

        if (e.target.name === "Dui") {

            let valor = e.target.value.replace(/\D/g, "");

            if (valor.length > 9) {
                valor = valor.substring(0, 9);
            }

            if (valor.length > 8) {
                valor = valor.substring(0, 8) + "-" + valor.substring(8);
            }

            e.target.value = valor;
        }
    });
});