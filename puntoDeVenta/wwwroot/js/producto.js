var datatable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Venta/obtenerProductos"
        },
        "columns": [
            { "data": "nombre", "width": "15%" },
            {
                "data": "imagen",
                "render": function (data) {
                    return `
                            <div>
                                <img src="${data}" width="50" height="50" />
                            </div>
                            `
                }, "width": "10%"
            },
            { "data": "nombreCategoria", "width": "10%" },
            { "data": "precioVenta", "width": "10%" },
            { "data": "stock", "width": "5%" },
            {
                "data": "productoID",
                "render": function (data) {
                    return `
                            <div>
                                <a href="/Venta/Agregar/${data}" class="btn btn-success text-white">
                                    <i class="fa-solid fa-plus"></i>  
                                </a>
                            </div>
                            `
                }, "width": "5%"
            }
        ]
    });
}