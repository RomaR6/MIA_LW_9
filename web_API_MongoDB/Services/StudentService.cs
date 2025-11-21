using web_API_MongoDB.Models;
using web_API_MongoDB.Repositories;

namespace web_API_MongoDB.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

       
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<List<Student>> GetAllAsync() =>
            await _studentRepository.GetAllAsync();

        public async Task<Student?> GetByIdAsync(string id) =>
            await _studentRepository.GetByIdAsync(id); 

        
        public async Task<Student?> GetByEmailAsync(string email) =>
            await _studentRepository.GetByEmailAsync(email);
       

        public async Task CreateAsync(Student student) =>
            await _studentRepository.CreateAsync(student);

        public async Task UpdateAsync(string id, Student studentIn) =>
            await _studentRepository.UpdateAsync(id, studentIn);

        public async Task DeleteAsync(string id) =>
            await _studentRepository.DeleteAsync(id);
    }
}