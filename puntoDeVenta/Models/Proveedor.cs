using System.ComponentModel.DataAnnotations;

namespace puntoDeVenta.Models
{
    public class Proveedor
    {
        public int ProveedorID { get; set; }
        [Required(ErrorMessage = "El nombre del proveedor es obligatorio")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "El ruc es obligatorio")]
        [MaxLength(11, ErrorMessage = "El ruc debe tener como máximo 11 caracteres")]
        public string ruc { get; set; }
        [Required(ErrorMessage = "La direccion es obligatorio")]
        public string direccion { get; set; }
        public ICollection<Producto>? productos { get; set; }
        public ICollection<Compra>? compras { get; set; }
    }
}
