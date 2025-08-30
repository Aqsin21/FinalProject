using Hospital.DAL.DataContext;
using Hospital.DAL.DataContext.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.UI.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Doctors = _context.Doctors.ToList();
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(
     string fullName,
     string phoneNumber,
     string email,
     string? address,
     int departmentId,
     int doctorId,
     DateTime appointmentDate)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Please fill in all required fields.");
                return View();
            }

            // Önce doktoru çek
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null)
            {
                ModelState.AddModelError("", "Selected doctor does not exist.");
                return View();
            }

            // Doktorun uygunluğunu kontrol et
            var isDoctorAvailable = !await _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId && a.AppointmentDate == appointmentDate);

            if (!isDoctorAvailable)
            {
                ModelState.AddModelError("", "Selected doctor is not available at this time.");
                return View();
            }
            var department = await _context.Departments.FindAsync(departmentId);
            if (department == null)
            {
                ModelState.AddModelError("", "Selected department does not exist.");
                return View();
            }

            var appointment = new Appointment
            {
                FullName = fullName,
                PhoneNumber = phoneNumber,
                Email = email,
                Address = address,
                DepartmentId = departmentId,
                Department = department, // << burayı ekledik
                DoctorId = doctorId,
                Doctor = doctor,         // zaten set edilmişti
                AppointmentDate = appointmentDate
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] = "Appointment successfully scheduled!";
            return Json(new { success = true, message = "Appointment successfully scheduled!" });
        }
    }
}