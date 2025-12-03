using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AgenciadeTours.Models.AgenciadeTours;

public partial class AgenciadeToursContext : DbContext
{
    public AgenciadeToursContext()
    {
    }

    public AgenciadeToursContext(DbContextOptions<AgenciadeToursContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Destino> Destinos { get; set; }

    public virtual DbSet<Pais> Paises { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-3G2DIGP;Database=AgenciadeTours;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Destino>(entity =>
        {
            entity.HasKey(e => e.DestinoId).HasName("PK__Destinos__4A838EF640EEFA78");

            entity.HasIndex(e => e.PaisId, "IX_Destinos_Paises");

            entity.Property(e => e.DestinoId).HasColumnName("DestinoID");
            entity.Property(e => e.DiasDuracion).HasColumnName("Dias_Duracion");
            entity.Property(e => e.HorasDuracion).HasColumnName("Horas_Duracion");
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.PaisId).HasColumnName("PaisID");

            entity.HasOne(d => d.Pais).WithMany(p => p.Destinos)
                .HasForeignKey(d => d.PaisId)
                .HasConstraintName("FK_Destinos_Paises");
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.HasKey(e => e.PaisId).HasName("PK__Paises__B501E1A5AF30474E");

            entity.Property(e => e.PaisId).HasColumnName("PaisID");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.TourId).HasName("PK__Tours__604CEA10E15C93B0");

            entity.HasIndex(e => e.DestinoId, "IX_Tours_Destinos");

            entity.HasIndex(e => e.PaisId, "IX_Tours_Paises");

            entity.Property(e => e.TourId).HasColumnName("TourID");
            entity.Property(e => e.DestinoId).HasColumnName("DestinoID");
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.PaisId).HasColumnName("PaisID");
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Destino).WithMany(p => p.Tours)
                .HasForeignKey(d => d.DestinoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tours_Destinos");

            entity.HasOne(d => d.Pais).WithMany(p => p.Tours)
                .HasForeignKey(d => d.PaisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tours_Paises");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
