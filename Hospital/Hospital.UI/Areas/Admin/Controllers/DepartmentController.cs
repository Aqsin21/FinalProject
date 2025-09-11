using Hospital.Business.Services.Abstract;
using Hospital.Business.Services.Concrete;
using Hospital.DAL.DataContext.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.UI.Areas.Admin.Controllers
{
    public class DepartmentController : AdminController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetAllAsync();
            return View(departments);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid) return View(department);
            if (await _departmentService.ExistsAsync(n => n.Name == department.Name))
            {
                ModelState.AddModelError("Title", "Bu isimde department mevcut.");
                return View(department);
            }

            await _departmentService.CreateAsync(department);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null) return NotFound();

            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            if (!ModelState.IsValid) return View(department);

            await _departmentService.UpdateAsync(department);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null) return NotFound();

            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
