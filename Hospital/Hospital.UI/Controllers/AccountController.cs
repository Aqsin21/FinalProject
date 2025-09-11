using Hospital.DAL.DataContext.Entities;
using Hospital.UI.Models;
using Mailing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Hospital.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
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
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email Doesn't Found");
                return View();
            }
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { resetToken, email }, Request.Scheme, Request.Host.ToString());
            _mailService.SendMail(new Mail { ToEmail = email, Subject = "Reset pas", TextBody = resetLink });
            return View();

        }
       

        [HttpGet]
        public IActionResult ResetPassword(string resetToken, string email)
        {
            var model = new ResetPasswordViewModel
            {
                ResetToken = resetToken,
                Email = email,
                NewPassword = "",
                ConfirmPassword = ""
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }
            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (user == null)
            {
                return BadRequest();


            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.ResetToken, resetPasswordViewModel.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                    ModelState.AddModelError("", item.Description);
                return View(resetPasswordViewModel);
            }
            return RedirectToAction(nameof(Login));

        }

    }
}