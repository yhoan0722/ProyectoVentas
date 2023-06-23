var datatable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Venta/obtenerListaVentas"
        },
        "columns": [
            { "data": "numeroVenta", "width": "5%" },
            { "data": "tipoVenta", "width": "5%" },
            { "data": "total", "width": "5%" },
            { "data": "fecha", "width": "10%" },
            {
                "data": "listaVentaID",
                "render": function (data) {
                    return `
                            <div>
                                <a href="/Venta/Detalle/${data}" class="btn btn-success text-white" style="cursor:pointer;">
                                <i class="fa-solid fa-circle-info"></i>
                                </a>
                            </div>
                            `
                },
                "width": "5%"
            }
        ]
    });
}