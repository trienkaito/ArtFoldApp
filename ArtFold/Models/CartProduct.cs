namespace ArtFold.Models
{
    public class CartProduct
    {
        public Guid CartID { get; set; }
        public Cart? Cart { get; set; }

        public Guid ProductID { get; set; }
        public Product? Product { get; set; }
        public int ProductCartQuantity {  get; set; }
    }
}
