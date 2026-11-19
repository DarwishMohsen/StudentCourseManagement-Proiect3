using PAW_Project.Models;

namespace PAW_Project.Repositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetAllWithDetailsAsync();
        Task<Course> GetCourseWithDetailsAsync(int id);
    }
}