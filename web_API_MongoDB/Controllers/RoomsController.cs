using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_API_MongoDB.Models;
using web_API_MongoDB.Services;

namespace web_API_MongoDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        
        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: api/rooms?dormitoryNumber=5&isAvailable=true
        
        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetAllRooms([FromQuery] int? dormitoryNumber, [FromQuery] bool? isAvailable)
        {
            var rooms = await _roomService.GetFilteredAsync(dormitoryNumber, isAvailable);
            return Ok(rooms);
        }

        // GET: api/rooms/60c72b2f5f1b2c...
        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Room>> GetRoomById(string id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room is null)
            {
                return NotFound("Кімнату з таким ID не знайдено.");
            }
            return Ok(room);
        }

        // POST: api/rooms
        
        [HttpPost]
        public async Task<IActionResult> CreateRoom(Room room)
        {
            await _roomService.CreateAsync(room);
            return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
        }

        // PUT: api/rooms/60c72b2f5f1b2c...
        
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateRoom(string id, Room roomIn)
        {
            var existingRoom = await _roomService.GetByIdAsync(id);
            if (existingRoom is null)
            {
                return NotFound("Кімнату з таким ID не знайдено.");
            }

            
            roomIn.Id = existingRoom.Id;

            await _roomService.UpdateAsync(id, roomIn);
            return NoContent();
        }

        // DELETE: api/rooms/60c72b2f5f1b2c...
        
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room is null)
            {
                return NotFound("Кімнату з таким ID не знайдено.");
            }

            try
            {
               
                await _roomService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }
    }
}