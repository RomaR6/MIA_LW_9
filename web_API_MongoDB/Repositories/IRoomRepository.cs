using web_API_MongoDB.Models;

namespace web_API_MongoDB.Repositories
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllAsync();
        Task<Room?> GetByIdAsync(string id);
        Task CreateAsync(Room room);
        Task UpdateAsync(string id, Room roomIn);
        Task DeleteAsync(string id);
    }
}