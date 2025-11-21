using web_API_MongoDB.Models;
using web_API_MongoDB.Repositories;

namespace web_API_MongoDB.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IBookingRepository _bookingRepository; 

        public RoomService(IRoomRepository roomRepository, IBookingRepository bookingRepository)
        {
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<List<Room>> GetAllAsync() =>
            await _roomRepository.GetAllAsync();

        
        public async Task<List<Room>> GetFilteredAsync(int? dormitoryNumber, bool? isAvailable)
        {
            var rooms = await _roomRepository.GetAllAsync();

            if (dormitoryNumber.HasValue)
            {
                rooms = rooms.Where(r => r.DormitoryNumber == dormitoryNumber.Value).ToList();
            }
            if (isAvailable.HasValue)
            {
                rooms = rooms.Where(r => r.IsAvailable == isAvailable.Value).ToList();
            }

            return rooms;
        }

        public async Task<Room?> GetByIdAsync(string id) =>
            await _roomRepository.GetByIdAsync(id);

        public async Task CreateAsync(Room room) =>
            await _roomRepository.CreateAsync(room);

        public async Task UpdateAsync(string id, Room roomIn) =>
            await _roomRepository.UpdateAsync(id, roomIn);

        
        public async Task DeleteAsync(string id)
        {
            var bookings = await _bookingRepository.GetByRoomIdAsync(id);
            if (bookings.Any())
            {
                
                throw new Exception("Неможливо видалити кімнату, оскільки вона заброньована.");
            }
            await _roomRepository.DeleteAsync(id);
        }
    }
}