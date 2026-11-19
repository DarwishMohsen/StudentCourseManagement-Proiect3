using System.ComponentModel.DataAnnotations;

namespace PAW_Project.Models
{
    public class Announcement
    {
        [Key]
        public int AnnouncementID { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CourseID { get; set; }
        public Course? Course { get; set; }

        public string AuthorId { get; set; } = string.Empty;
        public ApplicationUser? Author { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}