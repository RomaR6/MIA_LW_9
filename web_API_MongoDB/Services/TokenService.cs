using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using web_API_MongoDB.Models;
using web_API_MongoDB.Settings;

namespace web_API_MongoDB.Services
{
    /// <summary>
    /// Реалізує ITokenService. Створює JWT-токени.
    /// </summary>
    public class TokenService : ITokenService
    {
        
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Конструктор, який отримує налаштування через Dependency Injection
        /// </summary>
        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }

        /// <summary>
        /// Створює JWT-токен для вказаного студента
        /// </summary>
        public string GenerateToken(Student student)
        {
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, student.Id!), 
                new Claim(JwtRegisteredClaimNames.Email, student.Email!), 
                new Claim("firstName", student.FirstName!), 
                new Claim(ClaimTypes.Role, "Student") 
            };

            
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,       
                audience: _jwtSettings.Audience,   
                claims: claims,                    
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes), 
                signingCredentials: creds         
            );

            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}