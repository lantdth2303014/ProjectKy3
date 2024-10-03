namespace ProjectKy3.Models
{
    public class Return
    {
        public int ReturnId { get; set; }
        public long OrderId { get; set; }
        public string Reason { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } // Values: "requested", "approved", "rejected"

        // Navigation properties
        public Order Order { get; set; }
    }
}
