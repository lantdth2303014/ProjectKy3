using Microsoft.AspNetCore.JsonPatch;
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
    public class OrderController : ControllerBase
    {
        private readonly ProjectKy3DbContext _context;

        public OrderController(ProjectKy3DbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            // Include related entities like OrderItems and sort by OrderDate (newest first)
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Variant) // Include ProductVariant
                .OrderByDescending(o => o.OrderDate) // Sort by OrderDate in descending order (newest first)
                .ToListAsync();
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(long id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Variant) // Include ProductVariant
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/Order/confirm/{id}
        [HttpPost("confirm/{id}")]
        public async Task<IActionResult> ConfirmOrder(long id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Status = "Confirmed";

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order confirmed successfully." });
        }

        // POST: api/Order/deny/{id}
        [HttpPost("deny/{id}")]
        public async Task<IActionResult> DenyOrder(long id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Status = "Denied";

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order denied successfully." });
        }

        // POST: api/Order/return/{id}
        [HttpPost("return/{id}")]
        public async Task<IActionResult> ReturnOrder(long id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Status = "Returned";

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order returned successfully." });
        }

        // POST: api/Order/cancel/{id}
        [HttpPost("cancel/{id}")]
        public async Task<IActionResult> CancelOrder(long id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Status = "Cancelled";

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order cancelled successfully." });
        }

        // POST: api/Order/pickUp/{id}
        [HttpPost("pickUp/{id}")]
        public async Task<IActionResult> PickUpOrder(long id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Status = "Picked up";

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order picked up." });
        }

        // POST: api/Order/dispatch/{id}
        [HttpPost("dispatch/{id}")]
        public async Task<IActionResult> DispatchOrder(long id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Status = "Dispatched";

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order dispatched." });
        }

        // POST: api/Order/arrive/{id}
        [HttpPost("arrive/{id}")]
        public async Task<IActionResult> ArriveOrder(long id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Status = "Arrived";

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order arrived." });
        }

        // POST: api/Order/dispatchForDelivery/{id}
        [HttpPost("dispatchForDelivery/{id}")]
        public async Task<IActionResult> DispatchForDeliveryOrder(long id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Status = "Dispatched for delivery";

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order dispatched for delivery." });
        }

        // DELETE: api/Order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems) // Include related OrderItems
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            // Remove associated order items first
            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(long id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        // POST: api/Order/checkout
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto checkoutDto)
        {
            // Fetch cart items for the user
            var cartItems = await _context.CartItems
                .Include(ci => ci.Variant)
                .ThenInclude(v => v.Product)
                .Where(ci => ci.UserId == checkoutDto.UserId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return BadRequest("Your cart is empty.");
            }

            // Create a new Order
            var order = new Order
            {
                UserId = checkoutDto.UserId,
                ShippingMethod = checkoutDto.ShippingMethod,
                PaymentMethod = checkoutDto.PaymentMethod,
                TotalAmount = cartItems.Sum(ci => ci.Quantity * ci.Price),
                ShippingAddress = checkoutDto.ShippingAddress,
                BillingAddress = checkoutDto.BillingAddress,
                City = checkoutDto.City,
                OrderNote = checkoutDto.OrderNote,
                Telephone = checkoutDto.Telephone,
                Email = checkoutDto.Email,
                Name = checkoutDto.Name, // Store the customer's name
                Paid = checkoutDto.Paid
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Move CartItems to OrderItems
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    VariantId = cartItem.VariantId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price
                };

                _context.OrderItems.Add(orderItem);
            }

            // Save the OrderItems
            await _context.SaveChangesAsync();

            // Clear the cart for the user
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order placed successfully.", orderId = order.OrderId });
        }
    }
    public class CheckoutDto
    {
        public long UserId { get; set; }
        public string ShippingMethod { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public string City { get; set; }
        public string OrderNote { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool Paid { get; set; }
    }
}
