using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace kuafor.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Rol> Roller { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Admin> Adminler { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Salon> Salonlar { get; set; }
        public DbSet<Islem> Islemler { get; set; }
        public DbSet<CalisanUygunluk> CalisanUygunluklar { get; set; }
        public DbSet<Randevu> Randevular { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=LAPTOP-U8UD9GM4\\SQLEXPRESS01;Initial Catalog=projedb;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔹 Kalıtım - TPH yapılandırması
            modelBuilder.Entity<Kullanici>()
                .HasDiscriminator<int>("RolId")
                .HasValue<Admin>(1)
                .HasValue<Calisan>(2)
                .HasValue<Musteri>(3);

            // 🔹 Calisan <-> Islem (many-to-many)
            modelBuilder.Entity<Calisan>()
                .HasMany(c => c.Islemler)
                .WithMany(i => i.Calisanlar)
                .UsingEntity<Dictionary<string, object>>(
                    "CalisanIslem",
                    j => j
                        .HasOne<Islem>()
                        .WithMany()
                        .HasForeignKey("IslemId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<Calisan>()
                        .WithMany()
                        .HasForeignKey("CalisanId")
                        .OnDelete(DeleteBehavior.Restrict)
                );

            // 🔹 Randevu ilişkileri
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Musteri)
                .WithMany()
                .HasForeignKey(r => r.MusteriId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Calisan)
                .WithMany()
                .HasForeignKey(r => r.CalisanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Islem)
                .WithMany()
                .HasForeignKey(r => r.IslemId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Salon ilişkileri
            modelBuilder.Entity<Calisan>()
                .HasOne(c => c.Salon)
                .WithMany(s => s.Calisanlar)
                .HasForeignKey(c => c.SalonId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Islem>()
                .HasOne(i => i.Salon)
                .WithMany(s => s.Islemler)
                .HasForeignKey(i => i.SalonId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
