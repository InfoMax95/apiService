using apiService.Models;

namespace apiService.DTO
{
    public class PostDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public int AuthorID { get; set; }
    }

    public class PostToView 
    {
        public Post Posts { get; set; }
        public Author Author { get; set; }
        public Typology Typology { get; set; }
    }
}
