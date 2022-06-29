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
            AppUser newUser = new AppUser
            {
                FullName = user.FullName,
                UserName = user.Username,
                Email = user.Email,

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

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }
        public IActionResult Signin()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signin(SigninVM user)
        {
            AppUser userDb = await _userManager.FindByEmailAsync(user.Email);
            if (userDb == null)
            {
                ModelState.AddModelError("", "Email or password is not correct.");
                return View(user);
            }
          
            var signinResult = await _signInManager.PasswordSignInAsync(userDb.UserName,user.Password,user.isPersistent,lockoutOnFailure:true);
            if (signinResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Please try a few minutes later.");
                return View(user);
            }
            if (!signinResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong.");
                return View(user);
            }
            if (!userDb.isActivated)
            {
                ModelState.AddModelError("", "Please verify your account.");
                return View(user);
            }
            return RedirectToAction("Index", "Home");


        }
    }
}
