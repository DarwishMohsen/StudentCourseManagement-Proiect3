using System.ComponentModel.DataAnnotations;

namespace PAW_Project.Models
{
    public class Professor
    {
        [Key]
        public string PCNP { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public string? Email { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}