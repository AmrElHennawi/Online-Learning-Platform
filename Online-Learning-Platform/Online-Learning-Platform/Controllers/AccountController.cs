﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Online_Learning_Platform.DataContext;
using Online_Learning_Platform.Models;
using Online_Learning_Platform.ViewModels;

namespace Online_Learning_Platform.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly DataContext.AppContext _dbcontext;

		public AccountController(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager, DataContext.AppContext dbcontext)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_dbcontext = dbcontext;
		}

		[HttpGet]
		public async Task<IActionResult> Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new AppUser
				{
					UserName = model.Email,
					Email = model.Email
				};

				var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					return RedirectToAction("Login", "Account");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
					model.RememberMe, false);

				if (result.Succeeded)
				{
					return RedirectToAction("index", "Home");
				}
			}
			ModelState.AddModelError("", "email or password incorrect please try again");

			return View(model);
		}
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("index", "Home");
		}

		[AcceptVerbs("Get", "Post")]
		public async Task<IActionResult> IsEmailAcive(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);

			if (user == null)
			{
				return Json($"This email doesn't exist");
			}
			else
			{
				if (!user.IsActive)
				{
					return Json($"This email is not active");
				}
				return Json(true);
			}
		}
	}
}
