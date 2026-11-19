using PAW_Project.Models;

namespace PAW_Project.Services
{
    public interface IStudiesService
    {
        Task<IEnumerable<Studies>> GetAllStudiesAsync();
    }
}