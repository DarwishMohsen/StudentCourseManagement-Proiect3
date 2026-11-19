using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAW_Project.Data;
using PAW_Project.Models;

namespace PAW_Project.Controllers
{
    [Authorize]
    public class ClassroomController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClassroomController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Main Google Classroom-style Course Hub
        public async Task<IActionResult> Index(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.Professors)
                .Include(c => c.Announcements)
                    .ThenInclude(a => a.Author)
                .Include(c => c.Announcements)
                    .ThenInclude(a => a.Comments)
                        .ThenInclude(cm => cm.Author)
                .Include(c => c.Materials)
                .Include(c => c.Assignments)
                .Include(c => c.Comments.Where(cm => !cm.IsPrivate && cm.AnnouncementID == null && cm.AssignmentID == null))
                    .ThenInclude(cm => cm.Author)
                .FirstOrDefaultAsync(c => c.CourseID == courseId);

            if (course == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            bool isCourseProfessor = User.IsInRole("Admin") ||
                (user != null && course.Professors.Any(p => p.UserId == user.Id));

            ViewBag.IsCourseProfessor = isCourseProfessor;
            return View(course);
        }

        // Professor's own "My Courses" page
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> MyTeachingCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var prof = await _context.Professors
                .Include(p => p.Courses)
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            return View(prof?.Courses ?? new List<Course>());
        }

        // Professor posts an Announcement
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Professor,Admin")]
        public async Task<IActionResult> PostAnnouncement(int courseId, string title, string content)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(content))
            {
                var announcement = new Announcement
                {
                    CourseID = courseId,
                    Title = title.Trim(),
                    Content = content.Trim(),
                    AuthorId = user.Id,
                    CreatedAt = DateTime.Now
                };

                _context.Announcements.Add(announcement);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { courseId });
        }

        // Public Comment on the Course Stream
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostCourseComment(int courseId, string content)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!string.IsNullOrWhiteSpace(content))
            {
                var comment = new Comment
                {
                    CourseID = courseId,
                    Content = content.Trim(),
                    AuthorId = user.Id,
                    IsPrivate = false,
                    CreatedAt = DateTime.Now
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { courseId });
        }

        // Public Comment on a specific Announcement
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostAnnouncementComment(int courseId, int announcementId, string content)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!string.IsNullOrWhiteSpace(content))
            {
                var comment = new Comment
                {
                    CourseID = courseId,
                    AnnouncementID = announcementId,
                    Content = content.Trim(),
                    AuthorId = user.Id,
                    IsPrivate = false,
                    CreatedAt = DateTime.Now
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { courseId });
        }
    }
}