using PAW_Project.Models;

namespace PAW_Project.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<IEnumerable<Room>> GetRoomsByCourseIdAsync(int courseId);
        Task<Room> GetByIdAsync(int id);
        Task CreateAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(int id);
    }
}