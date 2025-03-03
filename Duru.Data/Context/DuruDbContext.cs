using Microsoft.EntityFrameworkCore;
using Duru.Domain.Models;

namespace Duru.Data.Context
{
    public class DuruDbContext : DbContext
    {
        public DuruDbContext(DbContextOptions<DuruDbContext> options) : base(options)
        {
            
        }
        // Tablolara karşılık gelen DbSet'ler
        public DbSet<Room> Rooms => Set<Room>();
    
        // İhtiyaç duyulan diğer DbSet'ler...
    }
}

