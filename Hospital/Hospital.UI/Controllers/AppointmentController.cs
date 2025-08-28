using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.UI.Controllers
{
    public class AppointmentController : Controller
    {
        [Authorize]

        public IActionResult Index()
        {
            return View();
        }
    }
}
