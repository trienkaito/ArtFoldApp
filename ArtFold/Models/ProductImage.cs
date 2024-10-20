namespace ArtFold.Models
{
    public class ProductImage
    {
        public Guid ProductImageID { get; set; } 
        public Guid ProductID { get; set; }
        public Product? Product { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }

    }

}
