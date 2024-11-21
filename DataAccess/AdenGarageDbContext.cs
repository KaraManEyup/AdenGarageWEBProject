using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AdenGarageWEB.DataAccess
{
    public class AdenGarageDbContext : DbContext
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

            modelBuilder.Entity<Araba>()
     .HasOne(a => a.Musteri)
     .WithMany(m => m.Arabalar)
     .HasForeignKey(a => a.MusteriId)
     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
