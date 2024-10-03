namespace ProjectKy3.Models
{
    public class CartItem
    {
        public long CartItemId { get; set; }
        public long UserId { get; set; }
        public long VariantId { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; }
        public ProductVariant Variant { get; set; }
    }
}
