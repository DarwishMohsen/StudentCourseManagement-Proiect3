using System.ComponentModel.DataAnnotations;

namespace PAW_Project.Models
{
    public class Assignment
    {
        [Key]
        public int AssignmentID { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CourseID { get; set; }
        public Course? Course { get; set; }

        public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}