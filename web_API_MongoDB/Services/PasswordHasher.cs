using System.Security.Cryptography;
using System.Text;

namespace web_API_MongoDB.Services
{
    /// <summary>
    /// Реалізує IPasswordHasher, використовуючи простий (але швидкий) алгоритм SHA256.
    /// Це відповідає вимогам методички.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// Хешує пароль за допомогою SHA256.
        /// </summary>
        /// <param name="password">Чистий пароль</param>
        /// <returns>Хеш у форматі Base64</returns>
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Перевіряє, чи збігається пароль з існуючим хешем.
        /// </summary>
        /// <param name="password">Пароль, який ввів користувач (чистий)</param>
        /// <param name="hash">Хеш, який зберігається в базі даних</param>
        /// <returns>True, якщо паролі збігаються, інакше false</returns>
        public bool Verify(string password, string hash)
        {
            // Щоб перевірити, ми просто хешуємо пароль,
            // який надав користувач, і порівнюємо його з хешем з бази даних.
            var computedHash = HashPassword(password);
            return computedHash == hash;
        }
    }
}