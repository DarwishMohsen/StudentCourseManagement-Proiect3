using PAW_Project.Models;

namespace PAW_Project.Services
{
    public interface IScheduleService
    {
        Task<IEnumerable<Schedule>> GetAllWithCoursesAsync();
        Task<Schedule> GetByIdAsync(int id);
        Task CreateAsync(Schedule schedule);
        Task UpdateAsync(Schedule schedule);
        Task DeleteAsync(int id);
        Task<Schedule> GetByIdWithCourseAsync(int id);
    }
}