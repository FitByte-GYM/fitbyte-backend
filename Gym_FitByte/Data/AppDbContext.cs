using Gym_FitByte.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym_FitByte.Data
{
    public class AppDbContext : DbContext
    {
        // 🔹 Constructor: recibe la configuración del contexto (cadena de conexión, etc.)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 🔹 Tablas que se crearán en la base de datos
        public DbSet<Membresia> Membresias { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<VentaVisita> VentasVisitas { get; set; }



        // 🔹 Sin configuración extra por ahora
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Puedes agregar configuraciones aquí si después necesitas relaciones o restricciones.
        }
    }
}
