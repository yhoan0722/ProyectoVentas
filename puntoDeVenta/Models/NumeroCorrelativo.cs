namespace puntoDeVenta.Models
{
    public class NumeroCorrelativo
    {
        public int NumeroCorrelativoID { get; set; }
        public int? UltimoNumero { get; set; }
        public int? CantidadDigitos { get; set; }
        public string? Gestion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
