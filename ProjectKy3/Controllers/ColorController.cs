using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectKy3.Data;
using ProjectKy3.Models;
using System.Threading.Tasks;

namespace ProjectKy3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly ProjectKy3DbContext _context;

        public ColorController(ProjectKy3DbContext context)
        {
            _context = context;
        }

        // GET: api/Color
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Color>>> GetColors()
        {
            return await _context.Colors.ToListAsync();
        }

        // GET: api/Color/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> GetColor(long id)
        {
            var color = await _context.Colors.FindAsync(id);

            if (color == null)
            {
                return NotFound("Color not found.");
            }

            return color;
        }

        // POST: api/Color
        [HttpPost]
        public async Task<ActionResult<Color>> PostColor(Color color)
        {
            _context.Colors.Add(color);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetColor), new { id = color.ColorId }, color);
        }

        // PUT: api/Color/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColor(long id, Color color)
        {
            if (id != color.ColorId)
            {
                return BadRequest("Color ID mismatch.");
            }

            _context.Entry(color).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColorExists(id))
                {
                    return NotFound("Color not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Color/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColor(long id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null)
            {
                return NotFound("Color not found.");
            }

            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ColorExists(long id)
        {
            return _context.Colors.Any(e => e.ColorId == id);
        }
    }
}
