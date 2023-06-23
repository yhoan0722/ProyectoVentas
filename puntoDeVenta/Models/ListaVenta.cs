namespace puntoDeVenta.Models
{
    public class ListaVenta
    {
        public int ListaVentaID { get; set; }
        public string? numeroVenta { get; set; }
        public string ? tipoVenta { get; set; }
        public double? total { get; set; }
        public DateTime? fecha { get; set; }
        public Ventas? venta { get; set; }
    }
}
