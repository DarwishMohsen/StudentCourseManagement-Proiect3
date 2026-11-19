using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAW_Project.Models;
using PAW_Project.Services;

namespace PAW_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProfessorsController : Controller
    {
        private readonly IProfessorService _service;
        public ProfessorsController(IProfessorService service) => _service = service;

        public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();
            var item = await _service.GetByIdAsync(id);
            return item == null ? NotFound() : View(item);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PCNP,LastName,FirstName")] Professor professor)
        {
            if (ModelState.IsValid) { await _service.CreateAsync(professor); return RedirectToAction(nameof(Index)); }
            return View(professor);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var item = await _service.GetByIdAsync(id);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PCNP,LastName,FirstName")] Professor professor)
        {
            if (id != professor.PCNP) return NotFound();
            if (ModelState.IsValid) { await _service.UpdateAsync(professor); return RedirectToAction(nameof(Index)); }
            return View(professor);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();
            var item = await _service.GetByIdAsync(id);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}