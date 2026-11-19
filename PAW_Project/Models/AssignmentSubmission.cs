using System.ComponentModel.DataAnnotations;

namespace PAW_Project.Models
{
    public class AssignmentSubmission
    {
        [Key]
        public int SubmissionID { get; set; }

        public int AssignmentID { get; set; }
        public Assignment? Assignment { get; set; }

        public string StudentId { get; set; } = string.Empty;
        public ApplicationUser? Student { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public float? Grade { get; set; }
        public string? Feedback { get; set; }
    }
}