using web_API_MongoDB.Models;

namespace web_API_MongoDB.Repositories
{
    /// <summary>
    /// Контракт для репозиторію студентів
    /// </summary>
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(string id);

        /// <summary>
        /// НОВИЙ МЕТОД: Знаходить студента за його email (для логіну)
        /// </summary>
        Task<Student?> GetByEmailAsync(string email);

        Task CreateAsync(Student student);
        Task UpdateAsync(string id, Student studentIn);
        Task DeleteAsync(string id);
    }
}