using System.Diagnostics;
using Hospital.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.UI.Controllers
{
    public class HomeController : Controller
    {
      

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        
    }
}
