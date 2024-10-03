namespace ProjectKy3.Models
{
    public class ProductVariant
    {
        public long VariantId { get; set; }
        public long ProductId { get; set; }
        public long ColorId { get; set; }
        public long SizeId { get; set; }
        public int StockQuantity { get; set; }

        // Navigation properties
        public Product Product { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }
    }
}
