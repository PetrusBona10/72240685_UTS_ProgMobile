using AcademicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicApi.Data
{
    public class AcademicDbContext : DbContext
    {
        public AcademicDbContext(DbContextOptions<AcademicDbContext> options)
            : base(options)
        {
        }

        public DbSet<Dosen> Dosen { get; set; }

        public DbSet<Mahasiswa> Mahasiswa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dosen>()
                .HasIndex(d => d.Nidn)
                .IsUnique();

            modelBuilder.Entity<Mahasiswa>()
                .HasIndex(m => m.Nim)
                .IsUnique();

            modelBuilder.Entity<Mahasiswa>()
                .HasOne(m => m.Dosen)
                .WithMany(d => d.Mahasiswa)
                .HasForeignKey(m => m.DosenId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
