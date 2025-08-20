using Hospital.Business.Services.Abstract;
using Hospital.Business.Services.Concrete;
using Hospital.DAL.DataContext.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital.UI.Areas.Admin.Controllers
{
    public class DoctorController : AdminController
    {
        private readonly IDoctorService _doctorService;
        private readonly IDepartmentService _departmentService;

        public DoctorController(IDoctorService doctorService, IDepartmentService departmentService)
        {
            _doctorService = doctorService;
            _departmentService = departmentService;
        }
        public async Task<IActionResult> Index()
        {
            // Department navigation property’sini Include ediyoruz
            var doctors = await _doctorService.GetAllAsync(includeProperties: "Department");
            return View(doctors);
        }



        [HttpGet]
            public async Task<IActionResult> Create()
            {
                var departments = await _departmentService.GetAllAsync();

                // Null kontrolü
                if (departments == null)
                    departments = new List<Department>();

                // ViewBag ile SelectList gönderiyoruz
                ViewBag.Departments = new SelectList(departments, "Id", "Name");

                return View();
            }

        [HttpPost]
        public async Task<IActionResult> Create(Doctor doctor, IFormFile? ImageFile)
        {
            // Model geçersizse, departments tekrar gönderiyoruz
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.GetAllAsync();
                ViewBag.Departments = new SelectList(departments, "Id", "Name");
                return View(doctor);
            }

            // Fotoğraf yükleme
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                doctor.ImagePath = "/uploads/" + fileName;
            }

            // Doctor kaydet
            await _doctorService.CreateAsync(doctor);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var departments = await _departmentService.GetAllAsync();

            // Null kontrolü
            if (departments == null)
                departments = new List<Department>();

            ViewBag.Departments = new SelectList(departments, "Id", "Name");

            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Doctor doctor, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                // ViewBag tekrar set edilmezse select list boş gelir
                var departments = await _departmentService.GetAllAsync();
                ViewBag.Departments = new SelectList(departments, "Id", "Name");
                return View(doctor);
            }

            var existingDoctor = await _doctorService.GetByIdAsync(doctor.Id);
            if (existingDoctor == null) return NotFound();

            // Alanları güncelle
            existingDoctor.FullName = doctor.FullName;
            existingDoctor.DepartmentId = doctor.DepartmentId;
            existingDoctor.Description = doctor.Description;

            // Fotoğraf varsa kaydet
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

                existingDoctor.ImagePath = "/uploads/" + fileName;
            }

            await _doctorService.UpdateAsync(existingDoctor);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id, includeProperties: "Department");
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _doctorService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

 }

