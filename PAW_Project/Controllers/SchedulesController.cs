using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAW_Project.Models;
using PAW_Project.Services;

namespace PAW_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SchedulesController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly ICourseService _courseService;

        public SchedulesController(IScheduleService scheduleService, ICourseService courseService)
        {
            _scheduleService = scheduleService;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _scheduleService.GetAllWithCoursesAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var schedule = await _scheduleService.GetByIdWithCourseAsync(id.Value);

            if (schedule == null) return NotFound();
            return View(schedule);
        }

        public async Task<IActionResult> Create()
        {
            var courses = await _courseService.GetAllAsync();
            ViewData["CourseID"] = new SelectList(courses, "CourseID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleID,Date,Hour,CourseID")] Schedule schedule)
        {

            schedule.Date = DateTime.SpecifyKind(schedule.Date, DateTimeKind.Utc);

            ModelState.Remove("Course");

            if (ModelState.IsValid)
            {
                await _scheduleService.CreateAsync(schedule);
                return RedirectToAction(nameof(Index));
            }

            var courses = await _courseService.GetAllAsync();
            ViewData["CourseID"] = new SelectList(courses, "CourseID", "Name", schedule.CourseID);
            return View(schedule);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var schedule = await _scheduleService.GetByIdAsync(id.Value);
            if (schedule == null) return NotFound();

            var courses = await _courseService.GetAllAsync();
            ViewData["CourseID"] = new SelectList(courses, "CourseID", "Name", schedule.CourseID);
            return View(schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleID,Date,Hour,CourseID")] Schedule schedule)
        {
            if (id != schedule.ScheduleID) return NotFound();

            schedule.Date = DateTime.SpecifyKind(schedule.Date, DateTimeKind.Utc);

            ModelState.Remove("Course");

            if (ModelState.IsValid)
            {
                try
                {
                    await _scheduleService.UpdateAsync(schedule);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    throw;
                }
            }

            var courses = await _courseService.GetAllAsync();
            ViewData["CourseID"] = new SelectList(courses, "CourseID", "Name", schedule.CourseID);
            return View(schedule);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var schedule = await _scheduleService.GetByIdWithCourseAsync(id.Value);

            if (schedule == null) return NotFound();
            return View(schedule);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _scheduleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}