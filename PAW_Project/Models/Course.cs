using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PAW_Project.Models
{
    public class Course
    {
        public Course()
        {
            Professors = new List<Professor>();
            Schedules = new List<Schedule>();
            StudentEnrollments = new List<Studies>();
        }

        [Key]
        public int CourseID { get; set; }
        public string Name { get; set; } = string.Empty;

        public int Year { get; set; }
        public int Semester { get; set; }

        public virtual ICollection<Professor> Professors { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }

        public virtual ICollection<Studies> StudentEnrollments { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
        public virtual ICollection<CourseMaterial> Materials { get; set; } = new List<CourseMaterial>();
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}