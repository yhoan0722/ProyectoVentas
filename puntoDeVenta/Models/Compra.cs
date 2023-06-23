using System.ComponentModel.DataAnnotations;

namespace puntoDeVenta.Models
{
    public class Compra
    {
        public int CompraID { get; set; }
        public string? numeroCompra { get; set; }
        [Required(ErrorMessage = "El nombre del proveedor es obligatorio")]
        public string nombreProveedor { get; set; }
        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        public string NombreProducto { get; set; }
        [Required(ErrorMessage = "El precio de compra es obligatorio")]
        public double precioCompra { get; set; }
        [Required(ErrorMessage = "La cantidad es obligatorio")]
        public int cantidad { get; set; }
        public DateTime fechaEntrega { get; set; }
        [Required(ErrorMessage = "El importe total es obligatorio")]
        public double importe { get; set; }
        public Proveedor? proveedor { get; set; }
        public Producto? producto { get; set; }
        public ICollection<ListaCompra>? compras { get; set; }
    }
}
