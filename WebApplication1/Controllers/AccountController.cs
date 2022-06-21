using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.ViewModels.Account;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager { get; }
        private SignInManager<AppUser> _signInManager { get; }
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            AppUser newUser = new AppUser {
                FullName = user.FullName,
                UserName=user.Username,
                Email=user.Email,
               
            };
            var identityResult = await _userManager.CreateAsync(newUser, user.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(user);
            }
            await _signInManager.SignInAsync(newUser, true);

            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");

        }
        public async Task<IActionResult> Signin()
        {
            return RedirectToAction("Index", "Home");

        }
    }
}
