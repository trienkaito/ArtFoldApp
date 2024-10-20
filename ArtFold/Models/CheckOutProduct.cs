namespace ArtFold.Models
{
    public class CheckOutProduct
    {
        public Guid CheckOutProductID { get; set; } 
        public Guid CheckOutID { get; set; }
        public CheckOut? CheckOut { get; set; }
        public Guid ProductID { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; } 
    }
}
