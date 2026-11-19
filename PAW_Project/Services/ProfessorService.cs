using PAW_Project.Models;
using PAW_Project.Repositories;

namespace PAW_Project.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly IProfessorRepository _repo;

        public ProfessorService(IProfessorRepository repo) => _repo = repo;

        public async Task<IEnumerable<Professor>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Professor> GetByIdAsync(string id) => await _repo.GetByIdAsync(id);

        public async Task CreateAsync(Professor professor)
        {
            await _repo.AddAsync(professor);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(Professor professor)
        {
            _repo.Update(professor);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var professor = await _repo.GetByIdAsync(id);
            if (professor != null)
            {
                _repo.Delete(professor);
                await _repo.SaveAsync();
            }
        }
    }
}