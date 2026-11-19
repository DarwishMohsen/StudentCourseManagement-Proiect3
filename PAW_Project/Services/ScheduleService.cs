using PAW_Project.Models;
using PAW_Project.Repositories;

namespace PAW_Project.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _repository;

        public ScheduleService(IScheduleRepository repository) => _repository = repository;

        public async Task<Schedule> GetByIdWithCourseAsync(int id) => await _repository.GetByIdWithCourseAsync(id);

        public async Task<IEnumerable<Schedule>> GetAllWithCoursesAsync() => await _repository.GetAllWithCoursesAsync();

        public async Task<Schedule> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task CreateAsync(Schedule schedule)
        {
            await _repository.AddAsync(schedule);
            await _repository.SaveAsync();
        }

        public async Task UpdateAsync(Schedule schedule)
        {
            _repository.Update(schedule);
            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var schedule = await _repository.GetByIdAsync(id);
            if (schedule != null)
            {
                _repository.Delete(schedule);
                await _repository.SaveAsync();
            }
        }
    }
}