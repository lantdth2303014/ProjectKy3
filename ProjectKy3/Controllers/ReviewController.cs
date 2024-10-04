using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectKy3.Data;
using ProjectKy3.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectKy3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ProjectKy3DbContext _context;

        public ReviewController(ProjectKy3DbContext context)
        {
            _context = context;
        }

        // GET: api/Review
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetAllReviews()
        {
            // Fetch all reviews from all products
            var reviews = await _context.Reviews
                .Include(r => r.User)      // Optionally include user details
                .Include(r => r.Product)   // Optionally include product details
                .ToListAsync();

            if (reviews == null || reviews.Count == 0)
            {
                return NotFound(new { message = "No reviews found." });
            }

            return Ok(reviews);
        }

        // POST: api/Review/{productId}
        [Authorize]
        [HttpPost("{productId}")]
        public async Task<IActionResult> PostReview(long productId, [FromBody] ReviewDto reviewDto)
        {
            // Extract the UserId from the authenticated user's claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid token.");
            }

            var userId = long.Parse(userIdClaim.Value);

            // Check if the product exists
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            // Create a new review
            var newReview = new Review
            {
                UserId = userId,
                ProductId = productId,  // ProductId from URL
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(newReview);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Review submitted successfully." });
        }
    }
}

public class ReviewDto
{
    public int Rating { get; set; }
    public string Comment { get; set; }
}
