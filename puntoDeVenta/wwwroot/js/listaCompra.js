var datatable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Compra/obtenerListaCompras"
        },
        "columns": [
            { "data": "numeroCompra", "width": "5%" },
            { "data": "total", "width": "5%" },
            { "data": "fecha", "width": "10%" },
            {
                "data": "listaCompraID",
                "render": function (data) {
                    return `
                            <div>
                                <a href="/Compra/Detalle/${data}" class="btn btn-success text-white" style="cursor:pointer;">
                                <i class="fa-solid fa-circle-info"></i>
                                </a>
                            </div>
                            `
                },
                "width": "5%"
            },
            {
                "data": "listaCompraID",
                "render": function (data) {
                    return `
                            <div>
                                <a onclick=Eliminar("/Compra/Eliminar/${data}") class="btn btn-danger text-white" style="cursor:pointer;">
                                <i class="fa-solid fa-trash-can"></i>
                                </a>
                            </div>
                            `
                },
                "width": "5%"
            }
        ]
    });
}
function Eliminar(url) {
    Swal.fire({
        title: 'Estas seguro de eliminar esta compra?',
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
                            'La compra seleccionada fue eliminada con exito.',
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