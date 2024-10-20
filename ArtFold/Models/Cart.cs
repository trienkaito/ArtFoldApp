namespace ArtFold.Models
{
    public class Cart
    {
        public Guid CartID { get; set; }
        public string UserID { get; set; }
        public User? User { get; set; }
        public ICollection<CartProduct>? CartProducts { get; set; }
    }
}   
