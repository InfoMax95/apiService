using Microsoft.EntityFrameworkCore;

namespace apiService.Models
{
    public class PostContext : DbContext
    {
        public PostContext(DbContextOptions<PostContext> options)
        : base(options)
        {
        }

        public DbSet<Post> TodoItems { get; set; } = null!;
    }
}
