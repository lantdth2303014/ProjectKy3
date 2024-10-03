using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectKy3.Data;
using ProjectKy3.Models;
using System.Threading.Tasks;

namespace ProjectKy3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnController : ControllerBase
    {
        private readonly ProjectKy3DbContext _context;

        public ReturnController(ProjectKy3DbContext context)
        {
            _context = context;
        }

        // POST: api/Return/{id}
        [HttpPost("{id}")]
        public async Task<IActionResult> RequestReturn(long id, [FromBody] ReturnReasonDto returnReasonDto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            var returnItem = new Return
            {
                OrderId = id,
                Reason = returnReasonDto.Reason,
                Status = "requested",
                RequestDate = DateTime.UtcNow
            };

            _context.Returns.Add(returnItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Return request submitted successfully.", returnId = returnItem.ReturnId });
        }

        // DELETE: api/Return/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReturn(int id)
        {
            var returnItem = await _context.Returns.FindAsync(id);
            if (returnItem == null)
            {
                return NotFound("Return request not found.");
            }

            _context.Returns.Remove(returnItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Return/{id}/approve
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveReturn(int id)
        {
            var returnItem = await _context.Returns.FindAsync(id);
            if (returnItem == null)
            {
                return NotFound("Return request not found.");
            }

            returnItem.Status = "Approved";
            _context.Entry(returnItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Return request approved." });
        }

        // PUT: api/Return/{id}/reject
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectReturn(int id)
        {
            var returnItem = await _context.Returns.FindAsync(id);
            if (returnItem == null)
            {
                return NotFound("Return request not found.");
            }

            returnItem.Status = "Rejected";
            _context.Entry(returnItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Return request rejected." });
        }
    }

    // DTO for return reason
    public class ReturnReasonDto
    {
        public string Reason { get; set; }
    }
}
