using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
    }

}
