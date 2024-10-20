namespace ArtFold.Models
{
    public class Comment
    {
        public Guid CommentID { get; set; }
        public string UserID { get; set; }
        public User? User { get; set; }
        public string Content { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public int Rating { get; set; } 
        public Guid ProductID { get; set; }
        public Product? Product { get; set; }
        public ICollection<CommentImage>? CommentImages { get; set; }
    }

}
