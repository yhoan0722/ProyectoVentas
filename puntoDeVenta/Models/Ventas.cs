namespace puntoDeVenta.Models
{
    public class Ventas
    {
        public int VentasID { get; set; }
        public int UsuarioID { get; set; }
        public string? numeroVenta { get; set; }
        public string nombreUsuario { get; set; }
        public string? nombreCliente { get; set; }
        public string nombreProducto { get; set; }
        public string nombreCategoria { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public double total { get; set; }
        public double igv { get; set; }
        public string tipoVenta { get; set; }
        public DateTime fecha { get; set; }
        public Usuario? usuario { get; set; }
        public ICollection<ListaVenta>? listaVentas { get; set; }
    }
}
