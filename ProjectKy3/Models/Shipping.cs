namespace ProjectKy3.Models
{
    public class Shipping
    {
        public int ShippingId { get; set; }
        public long OrderId { get; set; }
        public string Carrier { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        // Navigation properties
        public Order Order { get; set; }
    }
}
