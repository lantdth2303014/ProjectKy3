using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectKy3.Controllers;
using ProjectKy3.Data;
using ProjectKy3.Models;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ProjectKy3DbContext _context;

    public AuthController(ProjectKy3DbContext context)
    {
        _context = context;
    }

    // POST: api/Auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
        if (existingUser != null)
        {
            return BadRequest("Email is already in use.");
        }

        var newUser = new User
        {
            Name = registerDto.Name,
            Email = registerDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            Role = "customer"
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Customer registered successfully" });
    }

    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Role == "customer");

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            return Unauthorized("Invalid credentials.");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return Ok(new { message = "Login successful." });
    }

    // POST: api/Auth/logout
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Logout successful." });
    }
}
