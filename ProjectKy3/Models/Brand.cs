using System.Text.Json.Serialization;

namespace ProjectKy3.Models
{
    public class Brand
    {
        public long BrandId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        // Exclude from model binding
        [JsonIgnore]
        public List<Product>? Products { get; set; } = new List<Product>();
    }
}
