using Hospital.DAL.DataContext;
using Hospital.DAL.DataContext.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Hospital.UI.ViewComponents
{
    public class MyFavoriteDoctorsViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public MyFavoriteDoctorsViewComponent(AppDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return View(new List<Doctor>());

            // Navigation property kullanmadan sadece DoctorId ile sorgu
            var doctorIds = await _dbContext.Favorites
                .Where(f => f.UserId == user.Id)
                .Select(f => f.DoctorId)
                .ToListAsync();

            var doctors = await _dbContext.Doctors
                .Where(d => doctorIds.Contains(d.Id))
                .Include(d => d.Department)
                .ToListAsync();

            return View(doctors);
        }
    }
}