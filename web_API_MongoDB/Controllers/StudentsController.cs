using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_API_MongoDB.Models;
using web_API_MongoDB.Services;

namespace web_API_MongoDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
       

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(students);
        }

        // GET: api/students/{id}
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Student>> GetStudentById(string id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student is null)
            {
                return NotFound("Студента з таким ID не знайдено.");
            }
            return Ok(student);
        }

        // POST: api/students
        
        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            
            if (string.IsNullOrWhiteSpace(student.PasswordHash))
            {
                return BadRequest("PasswordHash не може бути порожнім.");
            }

            await _studentService.CreateAsync(student);

            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }

        // PUT: api/students/{id}
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateStudent(string id, Student studentIn)
        {
            var existingStudent = await _studentService.GetByIdAsync(id);
            if (existingStudent is null)
            {
                return NotFound("Студента з таким ID не знайдено.");
            }

           
            studentIn.Id = existingStudent.Id;
            studentIn.PasswordHash = existingStudent.PasswordHash;

            await _studentService.UpdateAsync(id, studentIn);
            return NoContent();
        }

        // DELETE: api/students/{id}
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student is null)
            {
                return NotFound("Студента з таким ID не знайдено.");
            }

            await _studentService.DeleteAsync(id);
            return NoContent();
        }
    }
}