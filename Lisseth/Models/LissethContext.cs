using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Lisseth.Models
{
    public partial class LissethContext : DbContext
    {
        public LissethContext()
        {
        }

        public LissethContext(DbContextOptions<LissethContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Cliente { get; set; } = null!;
        public virtual DbSet<Telefono> Telefono { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");

                entity.Property(e => e.Correo).HasMaxLength(100);

                entity.Property(e => e.Direccion).HasMaxLength(255);

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.Nombre).HasMaxLength(100);
            });

            modelBuilder.Entity<Telefono>(entity =>
            {
                entity.ToTable("Telefono");

                entity.Property(e => e.Compania).HasMaxLength(100);

                entity.Property(e => e.NumeroTelefono).HasMaxLength(20);

                entity.Property(e => e.Tipo).HasMaxLength(50);

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Telefono)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Telefono__IdClie__398D8EEE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
