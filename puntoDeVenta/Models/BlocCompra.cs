using System.ComponentModel.DataAnnotations;

namespace puntoDeVenta.Models
{
    public class BlocCompra
    {
        public int BlocCompraID { get; set; }
        public string? nombreProveedor { get; set; }
        public string? NombreProducto { get; set; }
        public double? precioCompra { get; set; }
        public int? cantidad { get; set; }
        public double? importe { get; set; }
    }
}
