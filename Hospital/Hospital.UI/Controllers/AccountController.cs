using Hospital.DAL.DataContext.Entities;
using Hospital.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        // Register GET
        public IActionResult Register()
        {
            return View();
        }

        // Register POST
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
                return View(registerViewModel);

            var user = new AppUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email
            };

            // Identity şifreyi hashleyip kaydeder
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerViewModel);
            }

            // Kullanıcı başarılı kayıt olduktan sonra Login sayfasına yönlendir
            return RedirectToAction("Login", "Account");
        }

        // Login GET
        public IActionResult Login()
        {
            return View();
        }

        // Login POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var existuser = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (existuser == null)
            {
                // Debug / test için ayrı mesaj, prod’da tek mesaj olmalı
                ModelState.AddModelError(string.Empty, "Username or Password is incorrect");
                return View(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(
                existuser,
                loginViewModel.Password,
                loginViewModel.RememberMe,
                false
            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username or Password is incorrect");
                return View(loginViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        // Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}