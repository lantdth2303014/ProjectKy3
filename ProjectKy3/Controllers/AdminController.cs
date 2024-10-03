using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectKy3.Data;
using ProjectKy3.Models;
using System.Security.Claims;

namespace ProjectKy3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ProjectKy3DbContext _context;

        public AdminController(ProjectKy3DbContext context)
        {
            _context = context;
        }

        // POST: api/Admin/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            // Check if the email is already in use
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                return BadRequest("Email is already in use.");
            }

            // Create a new admin user
            var newAdmin = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password), // Hash the password
                Role = "admin" // Assign admin role by default
            };

            _context.Users.Add(newAdmin);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Admin registered successfully" });
        }

        // POST: api/Admin/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            // Find the admin by email
            var admin = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Role == "admin");
            if (admin == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, admin.Password))
            {
                return Unauthorized("Invalid credentials.");
            }

            // Create the claims for the admin
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, admin.UserId.ToString()),
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim(ClaimTypes.Name, admin.Name),
                new Claim(ClaimTypes.Role, admin.Role)
            };

            // Create the identity
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Sign in using cookie authentication
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(new { message = "Admin logged in successfully." });
        }

        // POST: api/Admin/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Sign the admin out
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Admin logged out successfully." });
        }
    }

    public class UserRegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
