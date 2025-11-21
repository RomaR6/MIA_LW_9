using web_API_MongoDB.Models;
using web_API_MongoDB.Repositories;

namespace web_API_MongoDB.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingService(IBookingRepository bookingRepository, IStudentRepository studentRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _studentRepository = studentRepository;
            _roomRepository = roomRepository;
        }

        public async Task<List<Booking>> GetAllAsync() =>
            await _bookingRepository.GetAllAsync();

        public async Task<List<Booking>> GetFilteredAsync(string? studentId, string? roomId)
        {
            var bookings = await _bookingRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(studentId))
            {
                bookings = bookings.Where(b => b.StudentId == studentId).ToList();
            }
            if (!string.IsNullOrWhiteSpace(roomId))
            {
                bookings = bookings.Where(b => b.RoomId == roomId).ToList();
            }
            return bookings;
        }

        public async Task<Booking?> GetByIdAsync(string id) =>
            await _bookingRepository.GetByIdAsync(id);

        
        public async Task<Booking> CreateAsync(Booking booking)
        {
            
            var student = await _studentRepository.GetByIdAsync(booking.StudentId);
            if (student is null)
            {
                throw new Exception("Студента з таким ID не знайдено.");
            }

            
            var room = await _roomRepository.GetByIdAsync(booking.RoomId);
            if (room is null)
            {
                throw new Exception("Кімнату з таким ID не знайдено.");
            }

            
            if (!room.IsAvailable)
            {
                throw new Exception("Ця кімната вже заброньована.");
            }

            
            var studentBookings = await _bookingRepository.GetByStudentIdAsync(booking.StudentId);
            if (studentBookings.Any())
            {
                throw new Exception("Цей студент вже має активне бронювання.");
            }

            
            room.IsAvailable = false;
            await _roomRepository.UpdateAsync(room.Id!, room); 

            booking.BookingDate = DateTime.UtcNow;
            await _bookingRepository.CreateAsync(booking);

            return booking;
        }

        public async Task DeleteAsync(string id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking is null)
            {
                throw new Exception("Бронювання не знайдено.");
            }

            
            var room = await _roomRepository.GetByIdAsync(booking.RoomId);
            if (room is not null)
            {
                room.IsAvailable = true;
                await _roomRepository.UpdateAsync(room.Id!, room);
            }

            await _bookingRepository.DeleteAsync(id);
        }
    }
}