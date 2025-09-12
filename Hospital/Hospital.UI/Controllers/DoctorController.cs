using Hospital.DAL.DataContext;
using Hospital.DAL.DataContext.Entities;
using Hospital.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.UI.Controllers
{
    public class DoctorController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public DoctorController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _dbContext = context;
            _userManager = userManager;
        }

        
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var doctors = await _dbContext.Doctors
                .Include(d => d.Department)
                .OrderBy(d => d.Id)
                .Take(3) // ilk sayfada 3 doktor
                .ToListAsync();

            var userFavoriteIds = new List<int>();
            if (user != null)
            {
                userFavoriteIds = await _dbContext.Favorites
                    .Where(f => f.UserId == user.Id)
                    .Select(f => f.DoctorId)
                    .ToListAsync();
            }

            var vm = new DoctorListViewModel
            {
                Doctors = doctors,
                UserFavoriteDoctorIds = userFavoriteIds
            };

            ViewBag.Skip = doctors.Count; // 3
            return View(vm);
        }

        public async Task<IActionResult> LoadMore(int skip = 0, int take = 3)
        {
            var user = await _userManager.GetUserAsync(User);

            var doctors = await _dbContext.Doctors
                .Include(d => d.Department)
                .OrderBy(d => d.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var userFavoriteIds = new List<int>();
            if (user != null)
            {
                userFavoriteIds = await _dbContext.Favorites
                    .Where(f => f.UserId == user.Id)
                    .Select(f => f.DoctorId)
                    .ToListAsync();
            }

            if (!doctors.Any())
                return Content(""); // Daha fazla doktor yok

            var vm = new DoctorListViewModel
            {
                Doctors = doctors,
                UserFavoriteDoctorIds = userFavoriteIds
            };

            ViewBag.Skip = skip + doctors.Count;
            return PartialView("_DoctorListPartial", vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int doctorId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(); // AJAX için doğru response

            var favorite = await _dbContext.Favorites
                .FirstOrDefaultAsync(f => f.UserId == user.Id && f.DoctorId == doctorId);

            if (favorite != null)
            {
                _dbContext.Favorites.Remove(favorite);
            }
            else
            {
                _dbContext.Favorites.Add(new Favorite
                {
                    UserId = user.Id,
                    DoctorId = doctorId,
                    User = user
                });
            }

            await _dbContext.SaveChangesAsync();
            return Ok(new { success = true }); // AJAX response
        }

        // Load More için
      
    }
}