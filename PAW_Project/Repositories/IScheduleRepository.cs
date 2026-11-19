using PAW_Project.Models;

namespace PAW_Project.Repositories
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        Task<IEnumerable<Schedule>> GetAllWithCoursesAsync();
        Task<Schedule> GetByIdWithCourseAsync(int id);
    }
}