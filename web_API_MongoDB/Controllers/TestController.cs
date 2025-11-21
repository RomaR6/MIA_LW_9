using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace web_API_MongoDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Публічний ендпойнт, доступний всім.
        /// </summary>
        [HttpGet("public")]
        [AllowAnonymous] 
        public IActionResult PublicEndpoint()
        {
            return Ok("Цей ендпойнт ПУБЛІЧНИЙ і доступний всім.");
        }

        /// <summary>
        /// Приватний ендпойнт, доступний лише авторизованим користувачам.
        /// </summary>
        [Authorize] 
        [HttpGet("private")]
        public IActionResult PrivateEndpoint()
        {
            

            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var userEmail = User.FindFirstValue(ClaimTypes.Email); 
            var userName = User.FindFirstValue("firstName"); 

            return Ok($"Це ПРИВАТНИЙ ендпойнт! Ви увійшли як: {userName} (ID: {userId}, Email: {userEmail})");
        }
    }
}