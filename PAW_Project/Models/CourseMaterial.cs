using System.ComponentModel.DataAnnotations;

namespace PAW_Project.Models
{
    public class CourseMaterial
    {
        [Key]
        public int MaterialID { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.Now;

        public int CourseID { get; set; }
        public Course? Course { get; set; }

        public string UploadedById { get; set; } = string.Empty;
        public ApplicationUser? UploadedBy { get; set; }
    }
}