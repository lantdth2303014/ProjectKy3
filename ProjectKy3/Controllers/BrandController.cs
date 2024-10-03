using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectKy3.Data;
using ProjectKy3.Models;

namespace ProjectKy3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly ProjectKy3DbContext _context;

        public BrandController(ProjectKy3DbContext context)
        {
            _context = context;
        }

        // GET: api/Brand
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            return await _context.Brands.ToListAsync();
        }

        // GET: api/Brand/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(long id)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            return brand;
        }

        // PUT: api/Brand/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(long id, Brand brand)
        {
            if (id != brand.BrandId)
            {
                return BadRequest("BrandId mismatch.");
            }

            _context.Entry(brand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandExists(id))
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

        // POST: api/Brand
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBrand), new { id = brand.BrandId }, brand);
        }

        // DELETE: api/Brand/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(long id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BrandExists(long id)
        {
            return _context.Brands.Any(e => e.BrandId == id);
        }
    }
}
