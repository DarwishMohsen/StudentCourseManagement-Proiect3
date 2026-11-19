using System.ComponentModel.DataAnnotations;

namespace PAW_Project.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string AuthorId { get; set; } = string.Empty;
        public ApplicationUser? Author { get; set; }

        public int? CourseID { get; set; }
        public Course? Course { get; set; }

        public int? AnnouncementID { get; set; }
        public Announcement? Announcement { get; set; }

        public int? AssignmentID { get; set; }
        public Assignment? Assignment { get; set; }

        public bool IsPrivate { get; set; } = false;
        public string? TargetStudentId { get; set; }
        public ApplicationUser? TargetStudent { get; set; }
    }
}