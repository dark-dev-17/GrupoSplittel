using System;
using GPS_Logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GPS_Logic.Data
{
    public partial class DireccionContext : DbContext
    {
        public DireccionContext()
        {
        }

        public DireccionContext(DbContextOptions<DireccionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Direccion> Direccion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=192.168.31.29;Initial Catalog=GestionPersonal; Integrated Security=True; Connect timeout=30;Encrypt=False;TrustServerCertificate=false;ApplicationIntent=ReadWrite;MultiSubnetfailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Direccion>(entity =>
            {
                entity.HasKey(e => e.IdDireccion);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(90);
                    
            });

            modelBuilder.Entity<Direccion>()
                .HasOne(e => e.Sociedad)
                .WithMany(c => c.Direcciones);

            modelBuilder.Entity<Direccion>()
                .HasOne(e => e.DireccionPa)
                .WithOne().HasForeignKey<Direccion>(a => a.DireccionParent).OnDelete(DeleteBehavior.Restrict);


        }
    }
}
