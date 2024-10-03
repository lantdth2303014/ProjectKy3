namespace ProjectKy3.Models
{
    public class Review
    {
        public long ReviewId { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
