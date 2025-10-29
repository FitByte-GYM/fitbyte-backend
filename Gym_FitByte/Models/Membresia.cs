using System.ComponentModel.DataAnnotations;

namespace Gym_FitByte.Models
{
    public class Membresia
    {
        [Key]
        public int Id { get; set; }

        // 🔹 Código único de cliente (solo numérico)
        [Required]
        public string CodigoCliente { get; set; } = string.Empty;

        // 🔹 Datos personales
        [Required]
        public string Nombre { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        [Required]
        public string Correo { get; set; } = string.Empty;
        public string Rutina { get; set; } = string.Empty;
        public string EnfermedadesOLesiones { get; set; } = "Ninguna";

        // 🔹 Imagen
        public string FotoUrl { get; set; } = string.Empty;

        // 🔹 Datos de membresía
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime FechaVencimiento { get; set; }
        public string FormaPago { get; set; } = "Efectivo";
        public string Tipo { get; set; } = "Inscripción";
        public bool Activa { get; set; } = true;

        // 🔹 Control de pagos
        public decimal MontoPagado { get; set; }
    }
}
