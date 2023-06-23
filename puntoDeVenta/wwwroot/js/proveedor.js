var datatable;
$(document).ready(function () {
    loadDataTable();
    var id = document.getElementById("idProveedor");
    if (id.value > 0) {
        $('#myModal').modal('show');
    }
});
function limpiar() {
    var idProveedor = document.getElementById("idProveedor");
    var nombre = document.getElementById("nombre");
    var ruc = document.getElementById("ruc");
    var direccion = document.getElementById("direccion");
    idProveedor.value = 0;
    nombre.value = "";
    ruc.value = "";
    direccion.value = "";
}
function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Proveedor/obtenerTodos"
        },
        "columns": [
            { "data": "nombre", "width": "15%" },
            { "data": "ruc", "width": "10%" },
            { "data": "direccion", "width": "10%" },
            {
                "data": "proveedorID",
                "render": function (data) {
                    return `
                            <div>
                                <a href="/Proveedor/ListaProveedor/${data}" class="btn btn-success text-white" style="cursor:pointer;">
                                <i class="fa-solid fa-pen"></i>
                                </a>
                                <a onclick=Eliminar("/Proveedor/Eliminar/${data}") class="btn btn-danger text-white" style="cursor:pointer;">
                                <i class="fa-solid fa-trash-can"></i>
                                </a>
                            </div>
                            `
                },
                "width": "10%"
            }
        ]
    });
}
function Eliminar(url) {
    Swal.fire({
        title: '¿Estas seguro de eliminar este Proveedor?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si, Eliminar!'
    }).then((borrar) => {
        if (borrar.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        Swal.fire(
                            'Eliminado!',
                            'El Proveedor seleccionado fue eliminado con exito.',
                            'success'
                        )
                        datatable.ajax.reload();
                    } else {
                        alert(data.message);
                    }
                }
            });
        }
    });
}