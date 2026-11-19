using PAW_Project.Models;
using PAW_Project.Data;

namespace PAW_Project.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context) { }
    }
}