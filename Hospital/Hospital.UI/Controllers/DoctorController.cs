using Hospital.DAL.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using System.Threading.Tasks;

namespace Hospital.UI.Controllers
{
    public class DoctorController : Controller
    {
        private readonly AppDbContext _dbContext;

        public DoctorController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var doctors = await _dbContext.Doctors
                .Include(d => d.Department)
                .ToListAsync();

          return View(doctors);
        }
    }
}
