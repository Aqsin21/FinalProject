using Hospital.DAL.DataContext;
using Hospital.DAL.DataContext.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace Hospital.UI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context)
        {
            _context = context;
        }

        // Kullanıcıya tüm doktorları listele
        public async Task<IActionResult> Index()
        {
            var doctors = await _context.Doctors.ToListAsync();
            return View(doctors);
        }

        // Stripe Checkout session oluşturma
        [HttpPost]
        [IgnoreAntiforgeryToken] // AJAX ile test için
        public async Task<IActionResult> CreateCheckoutSession(
            [FromForm] string fullName,
            [FromForm] string email,
            [FromForm] int doctorId,
            [FromForm] int departmentId,
            [FromForm] DateTime appointmentDate,
            [FromForm] string? address)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
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
                            UnitAmountDecimal = doctor.Price * 100,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Consultation with Dr. {doctor.FullName}"
                            }
                        },
                        Quantity = 1
                    }
                },
                SuccessUrl = Url.Action("PaymentSuccess", "Payment",
                                new { doctorId, departmentId, fullName, email, appointmentDate, address },
                                Request.Scheme),
                CancelUrl = Url.Action("Cancel", "Payment", null, Request.Scheme)
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            // AJAX uyumlu: sadece session URL döndür
            return Content(session.Url);
        }

        // Başarılı ödeme sonrası → Appointment kaydı
        public async Task<IActionResult> PaymentSuccess(int doctorId, int departmentId, string fullName, string email, DateTime appointmentDate, string? address)
        {
            // Doctor ve Department nesnelerini çek
            var doctorEntity = await _context.Doctors.FindAsync(doctorId);
            if (doctorEntity == null) return NotFound();

            var selectedDepartment = await _context.Departments.FindAsync(departmentId);
            if (selectedDepartment == null) return NotFound();

            // Appointment oluştur
            var appointment = new Appointment
            {
                FullName = fullName,
                Email = email,
                DoctorId = doctorId,
                Doctor = doctorEntity,             // 🔹 Set et
                DepartmentId = departmentId,
                Department = selectedDepartment,   // 🔹 Set et
                AppointmentDate = appointmentDate,
                Address = address,
                Paid = true
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            ViewBag.Message = "✅ Payment successful! Your appointment is confirmed.";
            return View("Success");
        }
        public IActionResult Cancel()
        {
            ViewBag.Message = "❌ Payment was canceled. Please try again.";
            return View();
        }
    }
}
