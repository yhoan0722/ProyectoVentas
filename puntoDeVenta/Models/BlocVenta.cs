namespace puntoDeVenta.Models
{
    public class BlocVenta
    {
        public int BlocVentaID { get; set; }
        public string? numeroVenta { get; set; }
        public string nombreProducto { get; set; }
        public string imagen { get; set; }
        public string nombreCategoria { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public double total { get; set; }
        public double igv { get; set; }
        public DateTime fecha { get; set; }
    }
}
