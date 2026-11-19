using PAW_Project.Models;

namespace PAW_Project.Services
{
    public interface IProfessorService
    {
        Task<IEnumerable<Professor>> GetAllAsync();
        Task<Professor> GetByIdAsync(string id);
        Task CreateAsync(Professor professor);
        Task UpdateAsync(Professor professor);
        Task DeleteAsync(string id);
    }
}