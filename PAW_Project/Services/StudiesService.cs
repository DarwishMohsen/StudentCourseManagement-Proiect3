using PAW_Project.Models;
using PAW_Project.Repositories;

namespace PAW_Project.Services
{
    public class StudiesService : IStudiesService
    {
        private readonly IStudiesRepository _studiesRepo;

        public StudiesService(IStudiesRepository studiesRepo)
        {
            _studiesRepo = studiesRepo;
        }

        public async Task<IEnumerable<Studies>> GetAllStudiesAsync()
        {
            return await _studiesRepo.GetAllAsync();
        }
    }
}