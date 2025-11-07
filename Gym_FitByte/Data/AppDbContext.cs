using Gym_FitByte.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym_FitByte.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Membresia> Membresias { get; set; }
        public DbSet<MembresiaHistorial> MembresiasHistorial { get; set; }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<VentaVisita> VentasVisitas { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Progreso> Progresos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
 
            modelBuilder.Entity<Membresia>()
                .HasIndex(m => m.CodigoCliente)
                .IsUnique();

            
            modelBuilder.Entity<MembresiaHistorial>()
                .HasOne(h => h.Membresia)
                .WithMany(m => m.Historial!)
                .HasForeignKey(h => h.MembresiaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
