using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectKy3.Data;
using ProjectKy3.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ProjectKy3.DTOs;

namespace ProjectKy3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ProjectKy3DbContext _context;

        public UserController(ProjectKy3DbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile()
        {
            // Extract UserId from the JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid token.");
            }

            var userId = long.Parse(userIdClaim.Value);
            var user = await _context.Users
                .Include(u => u.Orders)
                .Include(u => u.CartItems)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Map the user data to UserProfileDto, excluding the password
            var userProfile = new UserProfileDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                ShippingAddress = user.ShippingAddress,
                BillingAddress = user.BillingAddress,
                State = user.State,
                City = user.City,
                Address = user.Address,
                Role = user.Role,
                DateOfBirth = user.DateOfBirth,
                Orders = user.Orders,
                CartItems = user.CartItems
            };

            return Ok(userProfile);
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
