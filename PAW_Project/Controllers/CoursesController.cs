using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAW_Project.Data;
using PAW_Project.Models;
using PAW_Project.Services;

namespace PAW_Project.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICourseService _service;
        private readonly IProfessorService _professorService;
        private readonly IRoomService _roomService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursesController(ICourseService service, IProfessorService professorService, IRoomService roomService, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _professorService = professorService;
            _roomService = roomService;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() => View(await _service.GetAllWithDetailsAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _service.GetCourseWithDetailsAsync(id.Value);
            return item == null ? NotFound() : View(item);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var rawProfs = await _professorService.GetAllAsync();
            var safeProfs = rawProfs?.Where(p => p != null && p.PCNP != null && p.LastName != null).ToList() ?? new List<Professor>();

            var rawRooms = await _roomService.GetAllAsync();
            var safeRooms = rawRooms?.Where(r => r != null && r.Building != null).ToList() ?? new List<Room>();

            ViewBag.Professors = new MultiSelectList(safeProfs, "PCNP", "LastName");
            ViewBag.Rooms = new SelectList(safeRooms, "RoomID", "Building");

            return View(new Course());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,Name,Year,Semester")] Course course, string[] professorIds, int? roomId)
        {
            ModelState.Remove("Professors");
            ModelState.Remove("Schedules");
            ModelState.Remove("StudentEnrollments");

            if (ModelState.IsValid)
            {
                await _service.CreateAsync(course, professorIds, roomId);
                return RedirectToAction(nameof(Index));
            }

            var rawProfs = await _professorService.GetAllAsync();
            ViewBag.Professors = new MultiSelectList(rawProfs ?? new List<Professor>(), "PCNP", "LastName", professorIds);
            var rawRooms = await _roomService.GetAllAsync();
            ViewBag.Rooms = new SelectList(rawRooms ?? new List<Room>(), "RoomID", "Building", roomId);

            return View(course);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var item = await _service.GetCourseWithDetailsAsync(id.Value);
            if (item == null) return NotFound();

            var rawProfs = await _professorService.GetAllAsync();
            var safeProfs = rawProfs?.Where(p => p != null && p.PCNP != null && p.LastName != null).ToList() ?? new List<Professor>();

            var rawRooms = await _roomService.GetAllAsync();
            var safeRooms = rawRooms?.Where(r => r != null && r.Building != null).ToList() ?? new List<Room>();

            var selectedProfIds = item.Professors?.Where(p => p != null).Select(p => p.PCNP).ToArray() ?? Array.Empty<string>();
            var currentRoomId = item.Schedules?.FirstOrDefault()?.Rooms?.FirstOrDefault(r => r != null)?.RoomID;

            ViewBag.Professors = new MultiSelectList(safeProfs, "PCNP", "LastName", selectedProfIds);
            ViewBag.Rooms = new SelectList(safeRooms, "RoomID", "Building", currentRoomId);

            return View(item);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,Name,Year,Semester")] Course course, string[] professorIds, int? roomId)
        {
            if (id != course.CourseID) return NotFound();

            ModelState.Remove("Professors");
            ModelState.Remove("Schedules");
            ModelState.Remove("StudentEnrollments");

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(course, professorIds, roomId);
                return RedirectToAction(nameof(Index));
            }

            var rawProfs = await _professorService.GetAllAsync();
            ViewBag.Professors = new MultiSelectList(rawProfs ?? new List<Professor>(), "PCNP", "LastName", professorIds);
            var rawRooms = await _roomService.GetAllAsync();
            ViewBag.Rooms = new SelectList(rawRooms ?? new List<Room>(), "RoomID", "Building", roomId);

            return View(course);
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var student = await _context.Students.FirstOrDefaultAsync(s => s.SCNP == user.UserName);

            if (student == null)
            {
                student = new Student
                {
                    SCNP = user.UserName,
                    LastName = "N/A",
                    FirstName = "N/A",
                    Year = 1,
                    Type = "Student"
                };
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
            }

            if (!_context.CourseStudies.Any(cs => cs.SCNP == student.SCNP && cs.CourseID == courseId))
            {
                _context.CourseStudies.Add(new Studies { SCNP = student.SCNP, CourseID = courseId, Grade = 0 });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("MyCourses", "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = await _service.GetCourseWithDetailsAsync(id.Value);
            return item == null ? NotFound() : View(item);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}