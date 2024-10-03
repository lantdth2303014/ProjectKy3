namespace ProjectKy3.Models
{
    public class Order
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string ShippingMethod { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "pending";
        public string Name { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public string City { get; set; }
        public string OrderNote { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public bool Paid { get; set; }

        // Navigation properties
        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
