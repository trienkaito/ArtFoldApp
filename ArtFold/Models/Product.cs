    namespace ArtFold.Models
    {
        public class Product
        {
            public Guid ProductID { get; set; }
            public Guid CategoryID { get; set; }
            public Category? Category { get; set; }
            public string Name { get; set; }
            public string ImgUrl { get; set; }
            public string PrintPaperType {  get; set; }
            public double Price {  get; set; }
            public int ProductQuantity {  get; set; }
            public string Description {  get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public ICollection<CartProduct>? CartProducts { get; set; }
            public ICollection<Comment>? Comments { get; set; }
            public ICollection<ProductImage>? ProductImages { get; set; }
    }
    }
