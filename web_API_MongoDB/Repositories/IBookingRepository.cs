using web_API_MongoDB.Models;

namespace web_API_MongoDB.Repositories
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(string id);
        Task<List<Booking>> GetByStudentIdAsync(string studentId);
        Task<List<Booking>> GetByRoomIdAsync(string roomId);
        Task CreateAsync(Booking booking);
        Task DeleteAsync(string id);
    }
}