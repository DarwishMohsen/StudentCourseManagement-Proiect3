using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAW_Project.Data;
using PAW_Project.Models;

namespace PAW_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index() => View();

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();
            var student = await _context.Students
                .Include(s => s.EnrolledCourses)
                .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.SCNP == user.UserName);

            return View(student?.EnrolledCourses);
        }

        public IActionResult Ghid() => View();
        public IActionResult Despre() => View();
        public IActionResult Contact() => View();
        public IActionResult Faq() => View();
    }
}