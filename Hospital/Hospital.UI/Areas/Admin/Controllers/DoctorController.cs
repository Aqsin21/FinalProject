using Hospital.Business.Services.Abstract;
using Hospital.DAL.DataContext.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe.Checkout;

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

        // Listeleme
        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorService.GetAllAsync(includeProperties: "Department");
            return View(doctors);
        }

        // Create GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.GetAllAsync() ?? new List<Department>();
            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            return View();
        }

        // Create POST
        [HttpPost]
        public async Task<IActionResult> Create(Doctor doctor, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.GetAllAsync();
                ViewBag.Departments = new SelectList(departments, "Id", "Name");
                return View(doctor);
            }

            if (await _doctorService.ExistsAsync(n => n.FullName == doctor.FullName))
            {
                ModelState.AddModelError("FullName", "Bu isimde bir doktor zaten mevcut.");
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

            // Price zaten modelden geliyor → direkt kaydedilecek
            await _doctorService.CreateAsync(doctor);
            return RedirectToAction(nameof(Index));
        }

        // Edit GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var departments = await _departmentService.GetAllAsync() ?? new List<Department>();
            ViewBag.Departments = new SelectList(departments, "Id", "Name");

            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        // Edit POST
        [HttpPost]
        public async Task<IActionResult> Edit(Doctor doctor, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.GetAllAsync();
                ViewBag.Departments = new SelectList(departments, "Id", "Name");
                return View(doctor);
            }

            var existingDoctor = await _doctorService.GetByIdAsync(doctor.Id);
            if (existingDoctor == null) return NotFound();

            existingDoctor.FullName = doctor.FullName;
            existingDoctor.DepartmentId = doctor.DepartmentId;
            existingDoctor.Description = doctor.Description;
            existingDoctor.Price = doctor.Price;

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

                existingDoctor.ImagePath = "/uploads/" + fileName;
            }

            await _doctorService.UpdateAsync(existingDoctor);
            return RedirectToAction(nameof(Index));
        }

        // Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id, includeProperties: "Department");
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        // Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _doctorService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Stripe Checkout - Doktor bazlı ödeme
        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession([FromForm] string fullName, [FromForm] string email, [FromForm] int doctorId)
        {
            // Doktoru getir
            var doctor = await _doctorService.GetByIdAsync(doctorId);
            if (doctor == null) return BadRequest("Doctor not found");

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                CustomerEmail = email,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = doctor.Price * 100, // Stripe cent formatı
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Consultation with Dr. {doctor.FullName}"
                            }
                        },
                        Quantity = 1
                    }
                },
                SuccessUrl = Url.Action("PaymentSuccess", "Doctor", new { doctorId, fullName, email }, Request.Scheme),
                CancelUrl = Url.Action("PaymentCancel", "Doctor", null, Request.Scheme)
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Redirect(session.Url);
        }

        // Ödeme başarılı
        public IActionResult PaymentSuccess(int doctorId, string fullName, string email)
        {
            TempData["SuccessMessage"] = $"Payment successful for Dr. {doctorId}, patient {fullName} ({email})!";
            return View();
        }

        // Ödeme iptal
        public IActionResult PaymentCancel()
        {
            TempData["ErrorMessage"] = "Payment was canceled.";
            return View();
        }
    }
}