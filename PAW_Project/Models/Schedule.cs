using System.ComponentModel.DataAnnotations;

namespace PAW_Project.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string Hour { get; set; }

        public int CourseID { get; set; }
        public Course? Course { get; set; }

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}