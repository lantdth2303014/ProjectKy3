namespace ProjectKy3.DTOs
{
    public class CheckoutDto
    {
        public long UserId { get; set; }
        public string ShippingMethod { get; set; }
        public string PaymentMethod { get; set; }
        public string Name { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? OrderNote { get; set; }
    }
}
