using Hospital.Business.Services;
using Hospital.Business.Services.Abstract;
using Hospital.DAL.DataContext.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomController : AdminController
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _roomService.GetAllAsync();
            return View(rooms);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Room room, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid) return View(room);

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                room.ImagePath = "/uploads/" + fileName;
            }

            await _roomService.CreateAsync(room);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null) return NotFound();
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Room room, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid) return View(room);

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                room.ImagePath = "/uploads/" + fileName;
            }

            await _roomService.UpdateAsync(room);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null) return NotFound();
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _roomService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
