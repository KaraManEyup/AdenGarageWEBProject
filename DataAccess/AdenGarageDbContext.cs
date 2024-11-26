using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdenGarageWEB.DataAccess
{
    public class AdenGarageDbContext : IdentityDbContext<ApplicationUser>
    {
        public AdenGarageDbContext(DbContextOptions<AdenGarageDbContext> options)
          : base(options)
        {
        }

        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Araba> Arabalar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Araba ve Musteri arasındaki ilişkiyi belirtmek
            modelBuilder.Entity<Araba>()
                .HasOne(a => a.Musteri)
                .WithMany(m => m.Arabalar)
                .HasForeignKey(a => a.MusteriId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Data Eklemek
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Rolleri Seed Etme
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
            );

            // Admin Kullanıcısını Seed Etme
            var adminUser = new ApplicationUser
            {
                Id = "admin-id", // Örnek bir admin id
                UserName = "admin@adengarage.com",
                NormalizedUserName = "ADMIN@ADENGARAGE.COM",
                Email = "admin@adengarage.com",
                NormalizedEmail = "ADMIN@ADENGARAGE.COM",
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "Admin Address",
                Gender = "Male"
            };

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // Admin kullanıcısını "Admin" rolüne eklemek
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = adminUser.Id,
                    RoleId = "1" // Admin rolü
                }
            );

            // Örnek bir User kullanıcısı Seed Etme
            var user = new ApplicationUser
            {
                Id = "user-id", // Örnek bir user id
                UserName = "user@adengarage.com",
                NormalizedUserName = "USER@ADENGARAGE.COM",
                Email = "user@adengarage.com",
                NormalizedEmail = "USER@ADENGARAGE.COM",
                FirstName = "User",
                LastName = "Example",
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1995, 5, 15),
                Address = "User Address",
                Gender = "Female"
            };

            modelBuilder.Entity<ApplicationUser>().HasData(user);

            // User kullanıcısını "User" rolüne eklemek
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = "2" // User rolü
                }
            );
        }
    }
}
