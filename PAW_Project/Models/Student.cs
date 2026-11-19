using System.ComponentModel.DataAnnotations;

namespace PAW_Project.Models
{
    public class Student
    {
        [Key]
        public string SCNP { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }

        public ICollection<Studies> EnrolledCourses { get; set; } = new List<Studies>();
    }
}