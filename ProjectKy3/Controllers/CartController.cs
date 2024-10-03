using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectKy3.Data;
using ProjectKy3.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectKy3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ProjectKy3DbContext _context;

        public CartController(ProjectKy3DbContext context)
        {
            _context = context;
        }

        // POST: api/Cart/add
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto cartDto)
        {
            var variant = await _context.ProductVariants
                .Include(v => v.Product)
                .FirstOrDefaultAsync(v => v.VariantId == cartDto.VariantId);

            if (variant == null)
            {
                return BadRequest("Product variant not found.");
            }

            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == cartDto.UserId && ci.VariantId == cartDto.VariantId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cartDto.Quantity;
                existingCartItem.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var newCartItem = new CartItem
                {
                    UserId = cartDto.UserId,
                    VariantId = cartDto.VariantId,
                    Quantity = cartDto.Quantity,
                    Price = variant.Product.Price, // Store product price
                    Image = variant.Product.Image  // Store product image
                };

                _context.CartItems.Add(newCartItem);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Item added to cart successfully." });
        }

        // GET: api/Cart/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetUserCart(long userId)
        {
            var cartItems = await _context.CartItems
                .Include(ci => ci.Variant)
                .ThenInclude(v => v.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (cartItems == null || !cartItems.Any())
            {
                return NotFound("Cart is empty.");
            }

            return Ok(cartItems);
        }

        // PUT: api/Cart/update/{cartItemId}
        [HttpPut("update/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(long cartItemId, [FromBody] UpdateCartDto updateCartDto)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            // Update cart item quantity
            cartItem.Quantity = updateCartDto.Quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cart item updated successfully." });
        }

        // DELETE: api/Cart/remove/{cartItemId}
        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> DeleteCartItem(long cartItemId)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cart item removed successfully." });
        }
    }

    public class AddToCartDto
    {
        public long UserId { get; set; }
        public long VariantId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartDto
    {
        public int Quantity { get; set; }
    }
}
