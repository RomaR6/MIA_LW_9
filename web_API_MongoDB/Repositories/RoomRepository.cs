using Microsoft.Extensions.Options;
using MongoDB.Driver;
using web_API_MongoDB.Models;
using web_API_MongoDB.Settings;

namespace web_API_MongoDB.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IMongoCollection<Room> _roomsCollection;

        public RoomRepository(IOptions<MongoDbSettings> settings)
        {
            // Передаємо налаштування у Singleton
            var context = MongoDbContext.GetInstance(settings.Value);
            _roomsCollection = context.GetCollection<Room>("Rooms");
        }

        public async Task<List<Room>> GetAllAsync() =>
            await _roomsCollection.Find(_ => true).ToListAsync();

        public async Task<Room?> GetByIdAsync(string id) =>
            await _roomsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Room room) =>
            await _roomsCollection.InsertOneAsync(room);

        public async Task UpdateAsync(string id, Room roomIn) =>
            await _roomsCollection.ReplaceOneAsync(x => x.Id == id, roomIn);

        public async Task DeleteAsync(string id) =>
            await _roomsCollection.DeleteOneAsync(x => x.Id == id);
    }
}