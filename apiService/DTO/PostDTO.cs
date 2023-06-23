using apiService.Models;

namespace apiService.DTO
{
    public class PostDTO
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Content { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string AuthorName { get; set; }
        public string Created { get; set; } = DateTime.UtcNow.ToLongDateString();
    }

    public class PostToView 
    {
        public Post Posts { get; set; }
        public Author Author { get; set; }
        public Typology Typology { get; set; }
    }
}
