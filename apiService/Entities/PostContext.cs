using apiService.Models;
using Microsoft.EntityFrameworkCore;

namespace apiService.Entities
{
    public class PostContext : DbContext
    {
        public PostContext(DbContextOptions<PostContext> options)
        : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; } = null!;
    }
}
