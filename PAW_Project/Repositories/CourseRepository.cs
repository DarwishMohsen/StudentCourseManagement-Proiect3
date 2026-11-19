using Microsoft.EntityFrameworkCore;
using PAW_Project.Data;
using PAW_Project.Models;

namespace PAW_Project.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Course>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(c => c.Professors)
                .Include(c => c.Schedules)
                    .ThenInclude(s => s.Rooms)
                .ToListAsync();
        }

        public async Task<Course> GetCourseWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Professors)
                .Include(c => c.Schedules)
                    .ThenInclude(s => s.Rooms)
                .FirstOrDefaultAsync(m => m.CourseID == id);
        }
    }
}