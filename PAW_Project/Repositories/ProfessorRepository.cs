using PAW_Project.Data;
using PAW_Project.Models;

namespace PAW_Project.Repositories
{
    public class ProfessorRepository : GenericRepository<Professor>, IProfessorRepository
    {
        public ProfessorRepository(ApplicationDbContext context) : base(context) { }
    }
}