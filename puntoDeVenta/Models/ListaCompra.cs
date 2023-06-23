using System.ComponentModel.DataAnnotations;

namespace puntoDeVenta.Models
{
    public class ListaCompra
    {
        public int ListaCompraID { get; set; }
        public string? numeroCompra { get; set; }
        public double? total { get; set; }
        public DateTime? fecha { get; set; }
        public Compra? compra { get; set; }
    }
}
