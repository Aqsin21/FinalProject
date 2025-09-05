using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.UI.Areas.Admin.Controllers
{
    
    public class DashBoardController : AdminController
    {
       
        public IActionResult Index()
        {
          
            return View();
        }
    }
}
