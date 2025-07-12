using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestAPIPrueba.Models;

public partial class ActividadUnidad4Context : DbContext
{
    public ActividadUnidad4Context()
    {
    }

    public ActividadUnidad4Context(DbContextOptions<ActividadUnidad4Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }
    public virtual DbSet<Inventario> Inventarios { get; set; }
    public virtual DbSet<Proveedor> Proveedores { get; set; }
    public virtual DbSet<Producto> Productos { get; set; }
    

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-D46TJT7;Database=ActividadUnidad4;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC07680432F8");
            entity.HasIndex(e => e.Code, "UQ__Producto__A25C5AA78989B5DE").IsUnique();
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Warehouse).HasMaxLength(100);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC07D21C930B");
            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D1053426E3BDE4").IsUnique();
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombreProveedor)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20);
            entity.Property(e => e.Correo)
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Cantidad).IsRequired();
            entity.Property(e => e.FechaEntrada).IsRequired();
            entity.Property(e => e.ProductoId).IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
