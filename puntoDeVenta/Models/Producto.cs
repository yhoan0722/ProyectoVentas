using System.ComponentModel.DataAnnotations;

namespace puntoDeVenta.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }
        [Required(ErrorMessage = "El nombre de la categoria es obligatorio")]
        [MaxLength(150, ErrorMessage = "El nombre de la categoria del producto debe tener como máximo 150 caracteres")]
        public string NombreCategoria { get; set; }
        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [MaxLength(150, ErrorMessage = "El nombre del producto debe tener como máximo 150 caracteres")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "El precio de compra producto es obligatorio")]
        public double precioCompra { get; set; }
        [Required(ErrorMessage = "El precio de venta producto es obligatorio")]
        public double precioVenta { get; set; }
        [Required(ErrorMessage = "El numero de stock es obligatorio")]
        public int stock { get; set; }
        public string estado { get; set; }
        public string? imagen { get; set; }
        public Categoria? categoria { get; set; }
        public Proveedor? proveedor { get; set;}
        public ICollection<Compra>? compras { get; set; }
    }
}
