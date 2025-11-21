using web_API_MongoDB.Models;

namespace web_API_MongoDB.Services
{
    /// <summary>
    /// Контракт для сервісу, що керує бізнес-логікою студентів
    /// </summary>
    public interface IStudentService
    {
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(string id);

        /// <summary>
        /// НОВИЙ МЕТОД: Знаходить студента за email (для логіну)
        /// </summary>
        Task<Student?> GetByEmailAsync(string email);

        Task CreateAsync(Student student);
        Task UpdateAsync(string id, Student studentIn);
        Task DeleteAsync(string id);
    }
}