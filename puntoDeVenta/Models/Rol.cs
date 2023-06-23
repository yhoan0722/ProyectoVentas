namespace puntoDeVenta.Models
{
    public class Rol
    {
        public int RolID { get; set; }
        public string? descripcion { get; set; }
        public ICollection<Usuario>? usuario { get; set; }
    }
}
