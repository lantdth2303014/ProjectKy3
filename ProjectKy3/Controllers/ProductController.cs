using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectKy3.Data;
using ProjectKy3.Models;

namespace ProjectKy3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProjectKy3DbContext _context;

        public ProductController(ProjectKy3DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.Include(p => p.Category).Include(p => p.Brand).ToListAsync();
        }

        // Search products by name, description, or category
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string? query, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            if (string.IsNullOrEmpty(query) && !minPrice.HasValue && !maxPrice.HasValue)
            {
                return BadRequest("At least one search parameter must be provided.");
            }

            var products = _context.Products
                .Include(p => p.Category)  // Include category for filtering by category name
                .Where(p =>
                    (string.IsNullOrEmpty(query) || p.Name.Contains(query) || p.Description.Contains(query) || p.Category.Name.Contains(query)) &&
                    (!minPrice.HasValue || p.Price >= minPrice) &&
                    (!maxPrice.HasValue || p.Price <= maxPrice));

            var result = await products.ToListAsync();

            if (!result.Any())
            {
                return NotFound("No products found matching the search query.");
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            var product = await _context.Products.Include(p => p.Category).Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            // Ensure navigation properties are not set during creation
            product.Brand = null; // Set to null to avoid the need for Brand during creation
            product.Category = null; // Same for Category

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(long id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
