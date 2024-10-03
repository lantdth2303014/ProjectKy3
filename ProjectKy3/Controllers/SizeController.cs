using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectKy3.Data;
using ProjectKy3.Models;
using System.Threading.Tasks;

namespace ProjectKy3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly ProjectKy3DbContext _context;

        public SizeController(ProjectKy3DbContext context)
        {
            _context = context;
        }

        // GET: api/Size
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Size>>> GetSizes()
        {
            return await _context.Sizes.ToListAsync();
        }

        // GET: api/Size/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Size>> GetSize(long id)
        {
            var size = await _context.Sizes.FindAsync(id);

            if (size == null)
            {
                return NotFound("Size not found.");
            }

            return size;
        }

        // POST: api/Size
        [HttpPost]
        public async Task<ActionResult<Size>> PostSize(Size size)
        {
            _context.Sizes.Add(size);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSize), new { id = size.SizeId }, size);
        }

        // PUT: api/Size/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSize(long id, Size size)
        {
            if (id != size.SizeId)
            {
                return BadRequest("Size ID mismatch.");
            }

            _context.Entry(size).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SizeExists(id))
                {
                    return NotFound("Size not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Size/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSize(long id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                return NotFound("Size not found.");
            }

            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SizeExists(long id)
        {
            return _context.Sizes.Any(e => e.SizeId == id);
        }
    }
}
