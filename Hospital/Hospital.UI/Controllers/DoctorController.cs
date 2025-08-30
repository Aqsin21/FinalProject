using Hospital.DAL.DataContext;
using Hospital.DAL.DataContext.Entities;
using Hospital.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using System.Threading.Tasks;

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

            return View(vm); // ViewModel gönderiyoruz
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToFavorite(int doctorId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index");

            // Favoride zaten var mı kontrol et
            var exists = await _dbContext.Favorites
                .AnyAsync(f => f.UserId == user.Id && f.DoctorId == doctorId);

            if (!exists)
            {
                _dbContext.Favorites.Add(new Favorite
                {
                    UserId = user.Id,
                    DoctorId = doctorId,
                    User = user

                });

                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int doctorId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index");

            var favorite = await _dbContext.Favorites
                .FirstOrDefaultAsync(f => f.UserId == user.Id && f.DoctorId == doctorId);

            if (favorite != null)
            {
                // Zaten favorideyse sil
                _dbContext.Favorites.Remove(favorite);
            }
            else
            {
                // Favoride değilse ekle
                _dbContext.Favorites.Add(new Favorite
                {
                    UserId = user.Id,
                    DoctorId = doctorId,
                    User = user
                });
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
