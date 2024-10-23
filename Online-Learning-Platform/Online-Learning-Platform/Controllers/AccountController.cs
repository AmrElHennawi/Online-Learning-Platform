using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_Learning_Platform.DataContext;
using Online_Learning_Platform.Interfaces;
using Online_Learning_Platform.Models;
using Online_Learning_Platform.ViewModels;

namespace Online_Learning_Platform.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IAccountService _accountService;

		public AccountController(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager, DBContext context, RoleManager<IdentityRole> roleManager, IAccountService accountService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_accountService = accountService;
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
				string type;
				if (_signInManager.IsSignedIn(User)) type = "Teacher";
				else type = "Student";

				var user = new AppUser
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					UserName = model.Email,
					Email = model.Email
				};


				var result = await _userManager.CreateAsync(user, model.Password);


				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(user, type);

					if (type == "Teacher") return RedirectToAction("ListUsers", "admin");
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

		[Authorize]
		public async Task<IActionResult> Profile(string id)
		{
			var profileView = await _accountService.GetProfileAsync(id, User);
			return View(profileView);
		}
	}
}

