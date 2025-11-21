namespace web_API_MongoDB.Settings
{
    /// <summary>
    /// Клас для "читання" налаштувань JWT з файлу appsettings.json
    /// </summary>
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenExpirationMinutes { get; set; }
    }
}