using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Learning_Platform.DataContext;
using Online_Learning_Platform.Interfaces;
using Online_Learning_Platform.Models;
using Online_Learning_Platform.ViewModels;

namespace Online_Learning_Platform.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DBContext _context;

        public AccountService(UserManager<AppUser> userManager,DBContext context)
        {
            _userManager = userManager;
            _context = context;
        }



        public async Task<ProfileViewModel> GetProfileAsync(string id, ClaimsPrincipal User)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != id)
            {
                throw new UnauthorizedAccessException();
            }
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var profileView = new ProfileViewModel()
            {
                User = user,
                Role = role
            };

            if (role == "Teacher")
            {
                profileView.Courses = await _context.Courses
                    .Include(c => c.CourseTeachers)
                    .Where(c => c.CourseTeachers.Any(ct => ct.TeacherId == id))
                    .ToListAsync();
            }
            else if (role == "Student")
            {
                profileView.Courses = await _context.Courses
                    .Include(c => c.Enrollments)
                    .Where(c => c.Enrollments.Any(ct => ct.StudentId == id))
                    .ToListAsync();
            }

            return profileView;
        }


    }
}
