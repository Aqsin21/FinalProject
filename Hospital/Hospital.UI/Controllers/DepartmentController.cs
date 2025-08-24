using Hospital.DAL.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hospital.UI.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _dbContext;

        public DepartmentController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _dbContext.Departments
                .Include(d => d.Doctors)
                .ToListAsync();
            return View(departments);
        }
    }
}
