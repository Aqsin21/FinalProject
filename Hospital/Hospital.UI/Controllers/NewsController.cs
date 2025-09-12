using Hospital.DAL.DataContext;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.UI.Controllers
{
    public class NewsController : Controller
    {
        private readonly AppDbContext _context;

        public NewsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var news = _context.Newss
                .OrderByDescending(n => n.Id)
                .Take(3)
                .ToList();

            return View(news);
        }

        public IActionResult LoadMore(int skip, int take)
        {
            var news = _context.Newss
                .OrderByDescending(n => n.Id)
                .Skip(skip)
                .Take(take)
                .ToList();

            if (!news.Any())
                return Content(""); // boşsa bitiriyoruz

            return PartialView("_NewsItems", news);
        }
    }
}
