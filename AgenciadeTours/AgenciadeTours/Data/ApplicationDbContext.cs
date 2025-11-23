using AgenciadeTours.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace AgenciadeTours.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Pais> Paises { get; set; }
        public DbSet<Destino> Destinos { get; set; }
        public DbSet<Tour> Tours { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pais>().HasIndex(c => c.Nombre).IsUnique(false);

            modelBuilder.Entity<Destino>()
                .HasOne(d => d.Pais)
                .WithMany(c => c.Destinos)
                .HasForeignKey(d => d.PaisID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tour>()
                .HasOne(t => t.Pais)
                .WithMany()
                .HasForeignKey(t => t.PaisID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tour>()
                .HasOne(t => t.Destino)
                .WithMany()
                .HasForeignKey(t => t.DestinoID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
