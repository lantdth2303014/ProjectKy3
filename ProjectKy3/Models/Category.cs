using System.Text.Json.Serialization;

namespace ProjectKy3.Models
{
    public class Category
    {
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }

        // Navigation properties
        // Exclude from model binding
        [JsonIgnore]
        public List<Product>? Products { get; set; } = new List<Product>();
    }
}
