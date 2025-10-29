using System.ComponentModel.DataAnnotations;

namespace Gym_FitByte.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Usuario { get; set; } = string.Empty;

        [Required]
        public string Contrasena { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Rol { get; set; } = "Admin";
    }
}
