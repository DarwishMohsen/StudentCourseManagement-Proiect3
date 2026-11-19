using PAW_Project.Models;
using PAW_Project.Data;

namespace PAW_Project.Repositories
{
    public class StudiesRepository : GenericRepository<Studies>, IStudiesRepository
    {
        public StudiesRepository(ApplicationDbContext context) : base(context) { }
    }
}