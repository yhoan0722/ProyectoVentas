var datatable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Compra/obtenerProductos"
        },
        "columns": [
            { "data": "nombre", "width": "10%" },
            { "data": "stock", "width": "5%" },
            {
                "data": "productoID",
                "render": function (data) {
                    return `
                            <div>
                                <a href="/Compra/AgregarCompra/${data}" class="btn btn-success text-white">
                                    <i class="fa-solid fa-plus"></i>    
                                </a>
                            </div>
                            `
                }, "width": "5%"
            }
        ]
    });
}