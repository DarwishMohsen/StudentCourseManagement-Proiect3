using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAW_Project.Models;
using PAW_Project.Services;

namespace PAW_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomsController : Controller
    {
        private readonly IRoomService _service;
        public RoomsController(IRoomService service) => _service = service;

        public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _service.GetByIdAsync(id.Value);
            return item == null ? NotFound() : View(item);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomID,Building")] Room room)
        {
            if (ModelState.IsValid) { await _service.CreateAsync(room); return RedirectToAction(nameof(Index)); }
            return View(room);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = await _service.GetByIdAsync(id.Value);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomID,Building")] Room room)
        {
            if (id != room.RoomID) return NotFound();
            if (ModelState.IsValid) { await _service.UpdateAsync(room); return RedirectToAction(nameof(Index)); }
            return View(room);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = await _service.GetByIdAsync(id.Value);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}