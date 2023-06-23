using System.ComponentModel.DataAnnotations;

namespace puntoDeVenta.Models
{
    public class Categoria
    {
        public int CategoriaID { get; set; }
        [Required(ErrorMessage = "El nombre de la categoria es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre de la categoria debe tener como máximo 100 caracteres")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "El estado de la categoria es obligatorio")]
        public string estado { get; set; }
        public ICollection<Producto>? productos { get; set; }
    }
}
