using System.ComponentModel.DataAnnotations;

namespace PAW_Project.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }
        public string Building { get; set; }

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}