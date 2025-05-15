using Gos_deneme.Models;
using Microsoft.EntityFrameworkCore;

namespace Gos_deneme.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<Kullaniciler> Kullaniciler { get; set; }
        public DbSet<Siparisler> Siparisler { get; set; }
        public DbSet<Odemeler> Odemeler { get; set; }
        public DbSet<Anlasmazliklar> Anlasmazliklar { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Anlasmazliklar>().HasKey(a => a.AnlasmazlikID);

            modelBuilder.Entity<Odemeler>()
                .HasOne(p => p.Siparisler)
                .WithMany(o => o.Odemeler)
                .HasForeignKey(p => p.SiparisID);

            modelBuilder.Entity<Odemeler>()
                .Property(p => p.OdemeTutar)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Anlasmazliklar>()
                .HasOne(d => d.Siparisler)
                .WithMany(o => o.Anlasmazliklar)
                .HasForeignKey(d => d.SiparisID);

            // 👇 Burada net olarak ilişkiler belirtiliyor
            modelBuilder.Entity<Siparisler>()
                .HasOne(s => s.Musteri)
                .WithMany()
                .HasForeignKey(s => s.MusteriID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Siparisler>()
                .HasOne(s => s.Satici)
                .WithMany()
                .HasForeignKey(s => s.SaticiID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
