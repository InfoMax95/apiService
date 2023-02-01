using apiService.Models;
using Microsoft.EntityFrameworkCore;

namespace apiService.Entities
{
    public class BlogApiContext : DbContext
    {
        public BlogApiContext(DbContextOptions<BlogApiContext> options)
        : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; } = null!;
    }
}
