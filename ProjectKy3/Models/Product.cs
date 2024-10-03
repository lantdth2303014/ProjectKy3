namespace ProjectKy3.Models
{
    public class Product
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public long CategoryId { get; set; } // Foreign key to Category
        public long BrandId { get; set; } // Foreign key to Brand

        // Optional navigation properties
        public Category? Category { get; set; } // Make nullable or ignore during creation
        public Brand? Brand { get; set; } // Make nullable or ignore during creation
        public List<ProductVariant>? ProductVariants { get; set; } = new List<ProductVariant>();
        public List<Review>? Reviews { get; set; } = new List<Review>();
        public List<Favorite>? Favorites { get; set; } = new List<Favorite>();
    }
}