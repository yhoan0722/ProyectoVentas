var datatable;
$(document).ready(function () {
    loadDataTable();
    var id = document.getElementById("idUsuario");
    if (id.value > 0) {
        $('#myModal').modal('show');
    }
});
function limpiar() {
    var idUsuario = document.getElementById("idUsuario");
    var nombre = document.getElementById("nombre");
    var correo = document.getElementById("correo");
    var clave = document.getElementById("clave");
    var dni = document.getElementById("dni");
    var telefono = document.getElementById("telefono");
    var estado = document.getElementById("estado");
    var rolId = document.getElementById("rolId");
    idUsuario.value = 0;
    nombre.value = "";
    correo.value = "";
    clave.value = "";
    dni.value = "";
    telefono.value = "";
    estado.value = "disponible";
    rolId.value = "";
}
function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Usuario/obtenerTodos"
        },
        "columns": [
            { "data": "nombre", "width": "15%" },
            {
                "data": "rolID",
                "render": function (data) {
                    if (data == 1) {
                        return "Administrativo";
                    }
                    if (data == 2) {
                        return "Empleado";
                    }
                }, "width": "10%",
            },
            { "data": "estado", "width": "10%" },
            { "data": "numeroDni", "width": "10%" },
            {
                "data": "usuarioID",
                "render": function (data) {
                    return `
                                <div>
                                    <a href="/Usuario/ListaUsuario/${data}" class="btn btn-success text-white">
                                        <i class="fa-solid fa-user-pen"></i>
                                    </a>
                                    <a onclick=Eliminar("/Usuario/Eliminar/${data}") class="btn btn-danger text-white">
                                        <i class="fa-solid fa-trash-can"></i>
                                    </a>
                                </div>
                            `
                }, "width": "10%"
            }
        ]
    });
}
function Eliminar(url) {
    swal({
        title: "Estas seguro de eliminar este Usuario?",
        text: "Este registro no se puede recuperar",
        icon: "warning",
        buttons: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        alert(data.message);
                        datatable.ajax.reload();
                    } else {
                        alert(data.message);
                    }
                }
            });
        }
    });
}