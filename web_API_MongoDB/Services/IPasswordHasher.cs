namespace web_API_MongoDB.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool Verify(string password, string hash);
    }
}