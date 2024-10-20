namespace ArtFold.Models
{
    public class CheckOut
    {
        public Guid CheckOutID {  get; set; }
        public string UserID { get; set; }
        public User? User { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; }
        public double TotalPrice { get; set; }
        public ICollection<CheckOutProduct>? CheckOutProducts { get; set; }

    }
}
