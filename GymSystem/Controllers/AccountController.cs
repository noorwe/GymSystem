using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.AccountViewModels;
using GymSystemDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace GymSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }

        #region Login Action

        public ActionResult Login() 
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            

            var User = _accountService.ValidateUser(model);
            if (User == null) 
            {
                ModelState.AddModelError("Data Invalid", "Invalid Email Or Password !");
                return View(model);
            }
            var Result = _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false).Result;

            if (Result.IsNotAllowed)
            {
                ModelState.AddModelError("", "You are not allowed to sign in. Please verify your email first.");
                return View(model);
            }
            if (Result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is locked. Try again later.");
                return View(model);
            }
            if (Result.Succeeded)
            {
                return RedirectToAction("Index", "Home"); 
            }

            return View(model);
        }

        #endregion



        #region Logout Action
        [HttpPost]
        public ActionResult Logout() 
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction("Login");
        }

        #endregion


        #region Access Denied Action

        public ActionResult AccessDenied()
        {
            return View();
        }

        #endregion
    }
}
