    using Hospital.DAL.DataContext;
    using Hospital.DAL.DataContext.Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Stripe.Checkout;

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
            [AutoValidateAntiforgeryToken]
            [HttpPost]
            public async Task<IActionResult> Create(IFormCollection form)
            {
                var departmentId = int.Parse(form["departmentId"]);
                var doctorId = int.Parse(form["doctorId"]);

                // Navigation entity’leri DB’den çek
                var department = _context.Departments.Find(departmentId);
                var doctor = _context.Doctors.Find(doctorId);

                // Appointment nesnesi oluştur
                var appointment = new Appointment
                {
                    FullName = form["fullName"],
                    PhoneNumber = form["phoneNumber"],
                    Email = form["email"],
                    Address = form["address"],
                    DepartmentId = departmentId,
                    DoctorId = doctorId,
                    Department = department!, // required
                    Doctor = doctor!,         // required
                    AppointmentDate = DateTime.Parse(form["appointmentDate"])
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                // 2. Stripe Checkout session oluştur
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = 5000, // Fiyatı kuruş cinsinden (50.00 USD)
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Doctor Appointment",
                        },
                    },
                    Quantity = 1,
                },
            },
                    Mode = "payment",
                    SuccessUrl = Url.Action("Success", "Appointment", new { id = appointment.Id }, Request.Scheme),
                    CancelUrl = Url.Action("Cancel", "Appointment", null, Request.Scheme),
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                // 3. Checkout URL’ini frontend’e gönder
                return Json(new { success = true, url = session.Url });
            }
        public async Task<IActionResult> Success(int id)
        {
            // Appointment objesini navigation property’leriyle birlikte çek
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Department)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

    }
    
    }