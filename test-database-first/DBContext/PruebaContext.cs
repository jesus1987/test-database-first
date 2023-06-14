using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_database_first.Models;

namespace test_database_first.DBContext;
public partial class PruebaContext : DbContext
    {
        public PruebaContext()
        {
        }

        public PruebaContext(DbContextOptions<PruebaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Local> Locals { get; set; }

        public virtual DbSet<Marca> Marcas { get; set; }

        public virtual DbSet<Producto> Productos { get; set; }

        public virtual DbSet<VentaDetalle> VentaDetalles { get; set; }

        public virtual DbSet<Venta> Venta { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=lab-defontana.caporvnn6sbh.us-east-1.rds.amazonaws.com,1433;Database=Prueba;User Id=ReadOnly;Password=d*3PSf2MmRX9vJtA5sgwSphCVQ26*T53uU; Trusted_Connection=True; TrustServerCertificate=True; Integrated Security = False;");
*/
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Local>(entity =>
            {
                entity.HasKey(e => e.IdLocal).HasName("PK__Local__3E34B29DF6370FC0");

                entity.ToTable("Local");

                entity.Property(e => e.IdLocal).HasColumnName("ID_Local");
                entity.Property(e => e.Direccion)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Nombre)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(e => e.IdMarca).HasName("PK__Marca__9B8F8DB2325A25B9");

                entity.ToTable("Marca");

                entity.Property(e => e.IdMarca).HasColumnName("ID_Marca");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto).HasName("PK__Producto__9B4120E21FBD1C85");

                entity.ToTable("Producto");

                entity.Property(e => e.IdProducto).HasColumnName("ID_Producto");
                entity.Property(e => e.Codigo)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.CostoUnitario).HasColumnName("Costo_Unitario");
                entity.Property(e => e.IdMarca).HasColumnName("ID_Marca");
                entity.Property(e => e.Modelo)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Nombre)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdMarca)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Producto__ID_Mar__52593CB8");
            });

            modelBuilder.Entity<VentaDetalle>(entity =>
            {
                entity.HasKey(e => e.IdVentaDetalle).HasName("PK__VentaDet__2F0CE38B52091CC3");

                entity.ToTable("VentaDetalle");

                entity.Property(e => e.IdVentaDetalle).HasColumnName("ID_VentaDetalle");
                entity.Property(e => e.IdProducto).HasColumnName("ID_Producto");
                entity.Property(e => e.IdVenta).HasColumnName("ID_Venta");
                entity.Property(e => e.PrecioUnitario).HasColumnName("Precio_Unitario");

                entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.VentaDetalles)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VentaDeta__ID_Pr__5DCAEF64");

                entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.VentaDetalles)
                    .HasForeignKey(d => d.IdVenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VentaDeta__ID_Ve__5CD6CB2B");
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(e => e.IdVenta).HasName("PK__Venta__3CD842E5A3F1C767");

                entity.Property(e => e.IdVenta).HasColumnName("ID_Venta");
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.IdLocal).HasColumnName("ID_Local");

                entity.HasOne(d => d.IdLocalNavigation).WithMany(p => p.Venta)
                    .HasForeignKey(d => d.IdLocal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Venta__ID_Local__571DF1D5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

