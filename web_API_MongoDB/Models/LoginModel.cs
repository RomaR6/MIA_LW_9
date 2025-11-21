namespace web_API_MongoDB.Models
{
    /// <summary>
    /// Модель, що використовується для прийому даних
    /// під час входу в систему (логіну).
    /// </summary>
    public class LoginModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}