using Fiorello_MVC.Models;
using Fiorello_MVC.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello_MVC.Controllers
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM )
        {
            if (!ModelState.IsValid) return View(registerVM);
            AppUser newUser = new AppUser
            {
                FullName=registerVM.FullName,
                Email=registerVM.Email,
                UserName=registerVM.Username,
                
            };
            IdentityResult result =await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                    return View(registerVM);
                }
            }
            await _signInManager.SignInAsync(newUser, false);
            return RedirectToAction("Index","Home");
        }
    }
}
