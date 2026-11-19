using PAW_Project.Models;
using PAW_Project.Repositories;

namespace PAW_Project.Services
{
    public class StudentService : IStudentService
    {
        private readonly IGenericRepository<Student> _studentRepo;
        private readonly IGenericRepository<Studies> _studiesRepo;

        public StudentService(IGenericRepository<Student> studentRepo, IGenericRepository<Studies> studiesRepo)
        {
            _studentRepo = studentRepo;
            _studiesRepo = studiesRepo;
        }

        public async Task EnrollStudentAsync(string cnp, string nume, string prenume, int courseId)
        {
            var student = new Student { SCNP = cnp, LastName = nume, FirstName = prenume, Year = 1, Type = "Regular" };
            await _studentRepo.AddAsync(student);

            var enrollment = new Studies { SCNP = cnp, CourseID = courseId, Grade = 0 };
            await _studiesRepo.AddAsync(enrollment);

            await _studentRepo.SaveAsync();
        }
    }
}