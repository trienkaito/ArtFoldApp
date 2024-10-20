namespace ArtFold.Models
{
    public class CommentImage
    {
        public Guid CommentImageID { get; set; }

        public string ImageUrl { get; set; }

        public Guid CommentID { get; set; }
        public Comment? Comment { get; set; } 
    }
}
