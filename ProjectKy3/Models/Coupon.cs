namespace ProjectKy3.Models
{
    public class Coupon
    {
        public int CouponId { get; set; }
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Requirement { get; set; }
        public long? ProductId { get; set; }

        // Navigation properties
        public Product Product { get; set; }
    }
}
