using Hospital.DAL.DataContext;
using Hospital.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
