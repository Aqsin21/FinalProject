using Hospital.DAL.DataContext;
using Hospital.DAL.DataContext.Entities;
using Hospital.UI.Areas.Admin.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital.UI.Areas.Admin.Controllers
{
    [SuperAdminOnly]
    public class AdminManagementController : AdminController
    {
        private readonly AppDbContext _context;

        public AdminManagementController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Tüm adminleri listele
        public IActionResult Index()
        {
            var admins = _context.AdminUsers.ToList();
            return View(admins);
        }

        public IActionResult Create()
        {
            // Enum değerlerini SelectListItem listesine dönüştür
            ViewBag.Roles = Enum.GetValues(typeof(AdminRole))
                                .Cast<AdminRole>()
                                .Select(r => new SelectListItem
                                {
                                    Text = r.ToString(),
                                    Value = r.ToString()
                                })
                                .ToList();

            return View();
        }

        // POST: Yeni admin ekle
        [HttpPost]
        public IActionResult Create(string username, string password, AdminRole role)
        {
            // Enum listesi tekrar view'a gönderilmeli (hata durumunda)
            ViewBag.Roles = Enum.GetValues(typeof(AdminRole))
                                .Cast<AdminRole>()
                                .Select(r => new SelectListItem
                                {
                                    Text = r.ToString(),
                                    Value = r.ToString()
                                })
                                .ToList();

            if (_context.AdminUsers.Any(u => u.UserName == username))
            {
                ViewBag.Error = "Bu kullanıcı adı zaten var!";
                return View();
            }

            var newAdmin = new AdminUser
            {
                UserName = username,
                Password = password,
                Role = role
            };

            _context.AdminUsers.Add(newAdmin);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Admin düzenleme sayfası
        // GET: Admin düzenleme sayfası
        public IActionResult Edit(int id)
        {
            var admin = _context.AdminUsers.FirstOrDefault(u => u.Id == id);
            if (admin == null)
                return NotFound();

            // Enum değerlerini SelectListItem listesine dönüştür
            ViewBag.Roles = Enum.GetValues(typeof(AdminRole))
                                .Cast<AdminRole>()
                                .Select(r => new SelectListItem
                                {
                                    Text = r.ToString(),
                                    Value = r.ToString(),
                                    Selected = (r == admin.Role)
                                })
                                .ToList();

            return View(admin);
        }

        // POST: Admin düzenle
        [HttpPost]
        public IActionResult Edit(int id, string username, string password, AdminRole role)
        {
            var admin = _context.AdminUsers.FirstOrDefault(u => u.Id == id);
            if (admin == null)
                return NotFound();

            // Enum listesi tekrar view'a gönderilmeli (hata durumunda)
            ViewBag.Roles = Enum.GetValues(typeof(AdminRole))
                                .Cast<AdminRole>()
                                .Select(r => new SelectListItem
                                {
                                    Text = r.ToString(),
                                    Value = r.ToString(),
                                    Selected = (r == role)
                                })
                                .ToList();

            admin.UserName = username;
            admin.Password = password;
            admin.Role = role;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin silme
        public IActionResult Delete(int id)
        {
            var admin = _context.AdminUsers.FirstOrDefault(u => u.Id == id);
            if (admin != null && admin.Role != AdminRole.SuperAdmin)
            {
                _context.AdminUsers.Remove(admin);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
