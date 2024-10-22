using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Learning_Platform.Models;

namespace Online_Learning_Platform.Controllers
{
	public class HomeController : Controller
	{
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
		{
            var user = await _userManager.GetUserAsync(User);

            if (_signInManager.IsSignedIn(User))    return View(user);

            return View();
		}

	}
}
