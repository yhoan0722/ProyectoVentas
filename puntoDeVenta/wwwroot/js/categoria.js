var datatable;
$(document).ready(function () {
    loadDataTable();
    var id = document.getElementById("idCategoria");
    if (id.value > 0) {
        $('#myModal').modal('show');
    }
});
function limpiar() {
    var idCategoria = document.getElementById("idCategoria");
    var nombre = document.getElementById("nombre");
    var estado = document.getElementById("estado");
    idCategoria.value = 0;
    nombre.value = "";
    estado.value = "disponible";
}
function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Categoria/ObtenerTodos"
        },
        "columns": [
            { "data": "nombre", "width": "15%" },
            { "data": "estado", "width": "10%" },
            {
                "data": "categoriaID",
                "render": function (data) {
                    return `
                            <div>
                                <a href="/Categoria/ListaCategoria/${data}" class="btn btn-success text-white" style="cursor:pointer;">
                                <i class="fa-solid fa-pen"></i>
                                </a>
                                <a onclick=Eliminar("/Categoria/Eliminar/${data}") class="btn btn-danger text-white" style="cursor:pointer;">
                                <i class="fa-solid fa-trash-can"></i>
                                </a>
                            </div>
                            `
                },
                "width": "20%"
            }
        ]
    });
}
function Eliminar(url) {
    Swal.fire({
        title: '¿Estas seguro de eliminar esta categoria?',
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
                            'La categoria seleccionada fue eliminada con exito.',
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