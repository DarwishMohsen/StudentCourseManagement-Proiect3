using Microsoft.EntityFrameworkCore;
using PAW_Project.Data;
using PAW_Project.Models;

namespace PAW_Project.Repositories
{
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(ApplicationDbContext context) : base(context) { }
        public async Task<IEnumerable<Schedule>> GetAllWithCoursesAsync()
        {
            return await _context.Schedules
                                 .Include(s => s.Course)
                                 .ToListAsync();
        }

        public async Task<Schedule> GetByIdWithCourseAsync(int id)
        {
            return await _context.Schedules
                                 .Include(s => s.Course)
                                 .FirstOrDefaultAsync(s => s.ScheduleID == id);
        }
    }
}