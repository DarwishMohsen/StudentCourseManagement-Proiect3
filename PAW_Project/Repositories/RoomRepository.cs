using PAW_Project.Data;
using PAW_Project.Models;

namespace PAW_Project.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext context) : base(context) { }
    }
}