using web_API_MongoDB.Models;

namespace web_API_MongoDB.Services
{
    public interface IRoomService
    {
        Task<List<Room>> GetAllAsync();
        Task<List<Room>> GetFilteredAsync(int? dormitoryNumber, bool? isAvailable);
        Task<Room?> GetByIdAsync(string id);
        Task CreateAsync(Room room);
        Task UpdateAsync(string id, Room roomIn);
        Task DeleteAsync(string id);
    }
}