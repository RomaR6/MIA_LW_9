using web_API_MongoDB.Models;

namespace web_API_MongoDB.Services
{
    /// <summary>
    /// Інтерфейс для сервісу, що генерує JWT-токени
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Створює JWT-токен для вказаного студента.
        /// </summary>
        /// <param name="student">Об'єкт студента</param>
        /// <returns>Рядок з JWT-токеном</returns>
        string GenerateToken(Student student);
    }
}