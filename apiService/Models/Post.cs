namespace apiService.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public int AuthorID { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }

    }
}
