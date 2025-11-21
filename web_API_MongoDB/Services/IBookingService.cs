using web_API_MongoDB.Models;

namespace web_API_MongoDB.Services
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAllAsync();
        Task<List<Booking>> GetFilteredAsync(string? studentId, string? roomId);
        Task<Booking?> GetByIdAsync(string id);
        Task<Booking> CreateAsync(Booking booking);
        Task DeleteAsync(string id);
    }
}