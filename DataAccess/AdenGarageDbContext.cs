using Core.Models;
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

            // Araba ve Musteri arasında ilişkileri yapılandırıyoruz
            modelBuilder.Entity<Araba>()
                .HasOne(a => a.Musteri)        // Bir araba bir müşteriye ait
                .WithMany(m => m.Arabalar)     // Bir müşteri birden fazla arabaSI olabilir 
                .HasForeignKey(a => a.MusteriId)  // Araba tablosunda MusteriId foreign key olarak tanımlanır
                .OnDelete(DeleteBehavior.Cascade);  // Müşteri silindiğinde ilgili arabalar da silinsin

            // Ek olarak, her modelin özelleştirilmiş kuralları varsa, burada tanımlanabilir.
        }
    }
}
