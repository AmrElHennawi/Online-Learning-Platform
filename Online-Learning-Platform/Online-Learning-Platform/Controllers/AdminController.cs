using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Online_Learning_Platform.DataContext;
using Online_Learning_Platform.Models;
using Online_Learning_Platform.ViewModels;

namespace Online_Learning_Platform.Controllers
{
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActive(string userId, bool active)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            user.IsActive = active; // Update the Active status
            var result = await userManager.UpdateAsync(user);

            return RedirectToAction("ListUsers");

        }

        [HttpGet]
        public async Task<IActionResult> ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditRolesInUser(string id)
        {
            ViewBag.userId = id;

            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required.");
            }

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            IList<string> roles = await userManager.GetRolesAsync(user);

            string role = string.Join(", ", roles);

            UserEditViewModel model = new UserEditViewModel
            {
                User = user,
                Role = role,
                SelectedRoles = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Student", Text = "Student" },
                    new SelectListItem { Value = "Teacher", Text = "Teacher" }
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRolesInUser(UserEditViewModel model, string NewRole, string userId)
        {
            model.SelectedRoles = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Student", Text = "Student" },
                    new SelectListItem { Value = "Teacher", Text = "Teacher" }
                };

            AppUser user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View(model);
            }

            var currentRoles = await userManager.GetRolesAsync(user);

            if (currentRoles[0] != NewRole)
            {

                if (currentRoles.Count > 0)
                {
                    await userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                if (!string.IsNullOrEmpty(NewRole))
                {
                    await userManager.AddToRoleAsync(user, NewRole);
                }
                return RedirectToAction("ListUsers");
            }
            else return View(model);

        }
    }
}
