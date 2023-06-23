using System.ComponentModel.DataAnnotations;

namespace puntoDeVenta.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public int RolID { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(200, ErrorMessage = "El nombre debe tener como máximo 200 caracteres")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio")]
        [MaxLength(100, ErrorMessage = "El correo debe tener como máximo 100 caracteres")]
        public string correo { get; set; }
        [Required(ErrorMessage = "La clave de usuario es obligatorio")]
        public string clave { get; set; }
        public string estado { get; set; }
        [MaxLength(8, ErrorMessage = "El DNI debe tener como máximo 8 digitos")]
        public string? numeroDni { get; set; }
        [Required(ErrorMessage = "La dirección es obligatorio")]
        public int? telefono { get; set; }
        public Rol? rol { get; set; }
        public ICollection<Ventas>? ventas { get; set; }
    }
}
