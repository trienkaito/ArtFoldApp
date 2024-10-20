namespace ArtFold.Models
{
    public class Category
    {
        public Guid CategoryID {  get; set; }
        public string CategoryName {  get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
