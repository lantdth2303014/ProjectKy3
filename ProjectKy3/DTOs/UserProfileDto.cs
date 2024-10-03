using ProjectKy3.Models;

namespace ProjectKy3.DTOs
{
    public class UserProfileDto
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string Role { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<Order> Orders { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
