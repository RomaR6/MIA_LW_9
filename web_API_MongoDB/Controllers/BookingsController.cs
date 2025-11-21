using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using web_API_MongoDB.Models;
using web_API_MongoDB.Services;
using System.Security.Claims;

namespace web_API_MongoDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: api/bookings
        
        [HttpGet]
        public async Task<ActionResult<List<Booking>>> GetAllBookings([FromQuery] string? studentId, [FromQuery] string? roomId)
        {
            var bookings = await _bookingService.GetFilteredAsync(studentId, roomId);
            return Ok(bookings);
        }

        // GET: api/bookings/60c72b2f5f1b2c...
        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Booking>> GetBookingById(string id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking is null)
            {
                return NotFound("Бронювання з таким ID не знайдено.");
            }
            return Ok(booking);
        }

        // POST: api/bookings
        
        [HttpPost]
        public async Task<IActionResult> CreateBooking(Booking booking)
        {
            
            // Ми більше не довіряємо 'StudentId' з тіла запиту (JSON).
            // Ми беремо ID студента (який зберігся в 'Sub' токена) прямо з токена.
            // Це гарантує, що студент може бронювати ТІЛЬКИ для себе.
            var studentIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (studentIdFromToken == null)
            {
                return Unauthorized("Не вдалося визначити ID користувача з токена.");
            }

            
            booking.StudentId = studentIdFromToken;
           

            try
            {
               
                var createdBooking = await _bookingService.CreateAsync(booking);

                return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBooking);
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/bookings/60c72b2f5f1b2c...
        
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            try
            {
                
                await _bookingService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
               
                return NotFound(ex.Message);
            }
        }
    }
}