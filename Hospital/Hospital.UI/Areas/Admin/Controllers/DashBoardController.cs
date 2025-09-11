using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin", AuthenticationSchemes = "AdminCookie")]
    public class DashBoardController : AdminController
    {
       
        public IActionResult Index()
        {
          
            return View();
        }
    }
}
