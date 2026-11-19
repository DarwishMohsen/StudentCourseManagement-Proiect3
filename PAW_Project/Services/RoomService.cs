using PAW_Project.Models;
using PAW_Project.Repositories;

namespace PAW_Project.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repo;

        public RoomService(IRoomRepository repo) => _repo = repo;

        public async Task<IEnumerable<Room>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Room> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task CreateAsync(Room room)
        {
            await _repo.AddAsync(room);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(Room room)
        {
            _repo.Update(room);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var room = await _repo.GetByIdAsync(id);
            if (room != null)
            {
                _repo.Delete(room);
                await _repo.SaveAsync();
            }
        }

        public async Task<IEnumerable<Room>> GetRoomsByCourseIdAsync(int courseId)
        {
            return await _repo.GetAllAsync();
        }
    }
}