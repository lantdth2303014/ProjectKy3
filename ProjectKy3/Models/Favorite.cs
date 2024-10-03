namespace ProjectKy3.Models
{
    public class Favorite
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
