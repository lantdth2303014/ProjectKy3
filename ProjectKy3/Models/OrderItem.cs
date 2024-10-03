namespace ProjectKy3.Models
{
    public class OrderItem
    {
        public long OrderItemId { get; set; }
        public long OrderId { get; set; }
        public long VariantId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public ProductVariant Variant { get; set; }
    }
}
