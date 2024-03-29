﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiService.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Display(Name = "Typology")]
        public virtual int Type { get; set; }
        [ForeignKey("Type")]
        public virtual Typology Typologies { get; set; }
        [Display(Name = "Author")]
        public virtual int AuthorID { get; set; }
        [ForeignKey("AuthorID")]
        public virtual Author Authors { get; set; }
        public List<Photo> Photos { get; set; } = new List<Photo>();
        public string Created_At { get; set; } = DateTime.UtcNow.ToLongDateString();
        public string Updated_At { get; set; } = DateTime.UtcNow.ToLongDateString();
    }
}
