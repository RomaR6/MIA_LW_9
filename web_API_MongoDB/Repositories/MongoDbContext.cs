using Microsoft.Extensions.Options;
using MongoDB.Driver;
using web_API_MongoDB.Settings;

namespace web_API_MongoDB.Repositories
{
    // Singleton: Забезпечує єдиний екземпляр підключення до MongoDB.
    public class MongoDbContext
    {
        private static MongoDbContext? _instance;
        private static readonly object _lock = new object();
        private readonly IMongoDatabase _database;

        // Приватний конструктор, приймає налаштування.
        private MongoDbContext(MongoDbSettings settings)
        {
            var mongoConnectionUrl = new MongoUrl(settings.ConnectionString);
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);

            // Налаштування SSL для MongoDB Atlas
            mongoClientSettings.SslSettings = new SslSettings() { CheckCertificateRevocation = false };

            var mongoClient = new MongoClient(mongoClientSettings);
            _database = mongoClient.GetDatabase(settings.DatabaseName);
        }

        // Статичний метод для отримання єдиного екземпляра (Потокобезпечний Singleton).
        public static MongoDbContext GetInstance(MongoDbSettings settings)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        // Тут створюємо єдиний екземпляр
                        _instance = new MongoDbContext(settings);
                    }
                }
            }
            return _instance;
        }

        // Надає колекцію для репозиторіїв.
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}