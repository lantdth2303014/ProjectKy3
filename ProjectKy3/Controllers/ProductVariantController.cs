using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectKy3.Data;
using ProjectKy3.Models;
using System.Threading.Tasks;

namespace ProjectKy3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private readonly ProjectKy3DbContext _context;

        public ProductVariantController(ProjectKy3DbContext context)
        {
            _context = context;
        }

        // GET: api/ProductVariant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVariant>>> GetProductVariants()
        {
            return await _context.ProductVariants
                                 .Include(pv => pv.Product)
                                 .Include(pv => pv.Color)
                                 .Include(pv => pv.Size)
                                 .ToListAsync();
        }

        // GET: api/ProductVariant/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVariant>> GetProductVariant(long id)
        {
            var productVariant = await _context.ProductVariants
                                               .Include(pv => pv.Product)
                                               .Include(pv => pv.Color)
                                               .Include(pv => pv.Size)
                                               .FirstOrDefaultAsync(pv => pv.VariantId == id);

            if (productVariant == null)
            {
                return NotFound("Product variant not found.");
            }

            return productVariant;
        }

        // POST: api/ProductVariant
        [HttpPost]
        public async Task<ActionResult<ProductVariant>> PostProductVariant([FromBody] ProductVariantDto variantDto)
        {
            // Check if the Product, Color, and Size exist
            var product = await _context.Products.FindAsync(variantDto.ProductId);
            var color = await _context.Colors.FindAsync(variantDto.ColorId);
            var size = await _context.Sizes.FindAsync(variantDto.SizeId);

            if (product == null || color == null || size == null)
            {
                return BadRequest("Invalid Product, Color, or Size.");
            }

            // Create the ProductVariant
            var productVariant = new ProductVariant
            {
                ProductId = variantDto.ProductId,
                ColorId = variantDto.ColorId,
                SizeId = variantDto.SizeId,
                StockQuantity = variantDto.StockQuantity
            };

            _context.ProductVariants.Add(productVariant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductVariant), new { id = productVariant.VariantId }, productVariant);
        }

        // PUT: api/ProductVariant/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductVariant(long id, [FromBody] ProductVariantDto variantDto)
        {
            if (id != variantDto.VariantId)
            {
                return BadRequest("Product variant ID mismatch.");
            }

            // Check if the Product, Color, and Size exist
            var product = await _context.Products.FindAsync(variantDto.ProductId);
            var color = await _context.Colors.FindAsync(variantDto.ColorId);
            var size = await _context.Sizes.FindAsync(variantDto.SizeId);

            if (product == null || color == null || size == null)
            {
                return BadRequest("Invalid Product, Color, or Size.");
            }

            // Update the ProductVariant
            var productVariant = await _context.ProductVariants.FindAsync(id);
            if (productVariant == null)
            {
                return NotFound("Product variant not found.");
            }

            productVariant.ProductId = variantDto.ProductId;
            productVariant.ColorId = variantDto.ColorId;
            productVariant.SizeId = variantDto.SizeId;
            productVariant.StockQuantity = variantDto.StockQuantity;

            _context.Entry(productVariant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductVariantExists(id))
                {
                    return NotFound("Product variant not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/ProductVariant/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductVariant(long id)
        {
            var productVariant = await _context.ProductVariants.FindAsync(id);
            if (productVariant == null)
            {
                return NotFound("Product variant not found.");
            }

            _context.ProductVariants.Remove(productVariant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductVariantExists(long id)
        {
            return _context.ProductVariants.Any(e => e.VariantId == id);
        }
    }

    public class ProductVariantDto
    {
        public long VariantId { get; set; }  // Optional for PUT requests
        public long ProductId { get; set; }
        public long ColorId { get; set; }
        public long SizeId { get; set; }
        public int StockQuantity { get; set; }
    }
}
