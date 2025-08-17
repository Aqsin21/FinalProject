using Hospital.DAL.DataContext.Entities;
using Hospital.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;



        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = new AppUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,



            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            return RedirectToAction("Login", "Account");


        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existuser = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (existuser == null)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(existuser, loginViewModel.Password, loginViewModel.RememberMe, false);
            if (!result.Succeeded)
            {

                ModelState.AddModelError("", "Username or Password is incorrect");

            }

            return RedirectToAction("Index", "Home");
        }
    }
}
