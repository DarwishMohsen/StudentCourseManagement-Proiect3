using PAW_Project.Models;

namespace PAW_Project.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<IEnumerable<Course>> GetAllWithDetailsAsync();
        Task<Course> GetByIdAsync(int id);
        Task<Course> GetCourseWithDetailsAsync(int id);
        Task CreateAsync(Course course, string[] professorIds, int? roomId);
        Task UpdateAsync(Course course, string[] professorIds, int? roomId);
        Task DeleteAsync(int id);
    }
}