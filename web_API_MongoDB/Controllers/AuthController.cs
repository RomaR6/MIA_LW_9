using Microsoft.AspNetCore.Mvc;
using web_API_MongoDB.Models;
using web_API_MongoDB.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; 

namespace web_API_MongoDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] 
    public class AuthController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthController(
            IStudentService studentService,
            ITokenService tokenService,
            IPasswordHasher passwordHasher)
        {
            _studentService = studentService;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Реєстрація нового студента (користувача).
        /// Пароль очікується у полі 'passwordHash'.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Student student)
        {
            if (student == null || string.IsNullOrEmpty(student.Email) || string.IsNullOrEmpty(student.PasswordHash))
            {
                return BadRequest("Email та PasswordHash є обов'язковими.");
            }

            var existingUser = await _studentService.GetByEmailAsync(student.Email);
            if (existingUser != null)
                return BadRequest("Студент з таким email вже існує.");

            
            // Ми беремо пароль, який користувач ввів у полі 'PasswordHash',
            // хешуємо його за допомогою SHA256 і кладемо назад у це ж поле.
            student.PasswordHash = _passwordHasher.HashPassword(student.PasswordHash);

            // Встановлюємо Id в null, щоб MongoDB згенерував новий
            student.Id = null;

            await _studentService.CreateAsync(student);

            
            student.PasswordHash = string.Empty;

            return CreatedAtAction(nameof(Register), new { id = student.Id }, student);
        }

        /// <summary>
        /// Вхід в систему та отримання JWT-токена
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _studentService.GetByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized("Неправильна пошта або пароль.");

            
            if (!_passwordHasher.Verify(model.Password, user.PasswordHash))
                return Unauthorized("Неправильна пошта або пароль.");

            // Якщо все добре, генеруємо токен
            var token = _tokenService.GenerateToken(user);

            return Ok(new { token = token });
        }
    }
}