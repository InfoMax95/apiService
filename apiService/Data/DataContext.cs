using apiService.Models;
using Microsoft.EntityFrameworkCore;

namespace apiService.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Typology> Typologies { get; set; }

    }
}
