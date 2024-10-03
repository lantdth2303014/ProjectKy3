using System.Text.Json.Serialization;

namespace ProjectKy3.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string Role { get; set; } = "customer";
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties (these should be ignored during API request)
        [JsonIgnore]
        public List<Order>? Orders { get; set; }

        [JsonIgnore]
        public List<CartItem>? CartItems { get; set; }

        [JsonIgnore]
        public List<Review>? Reviews { get; set; }

        [JsonIgnore]
        public List<Favorite>? Favorites { get; set; }
    }
}
