using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GPS_Logic.Data
{
    public partial class GestionPersonalContext : DbContext
    {
        public GestionPersonalContext()
        {
        }

        public GestionPersonalContext(DbContextOptions<GestionPersonalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Sociedad> Sociedad { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=192.168.31.29;Initial Catalog=GestionPersonal; Integrated Security=True; Connect timeout=30;Encrypt=False;TrustServerCertificate=false;ApplicationIntent=ReadWrite;MultiSubnetfailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sociedad>(entity =>
            {
                entity.HasKey(e => e.IdSociedad);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(90);

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(255);
            });
        }
    }
}
