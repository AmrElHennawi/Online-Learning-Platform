using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Learning_Platform.Models;
using Online_Learning_Platform.DataContext;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Online_Learning_Platform.Services
{
    public class PermissionService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DBContext _context;

        public PermissionService(UserManager<AppUser> userManager, DBContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> CheckTypeAsync(ClaimsPrincipal userprincipal, string type)
        {
            var user = await _userManager.GetUserAsync(userprincipal);

            bool res = await _userManager.IsInRoleAsync(user, type);
            return res;
        }

        public async Task<bool> CheckTeacherCourseAsync(ClaimsPrincipal userprincipal, Course Course)
        {
            var user = await _userManager.GetUserAsync(userprincipal);

            var isfound = Course.CourseTeachers.FirstOrDefault(ct => ct.TeacherId == user.Id);

            return isfound != null;
        }
        public async Task<bool> CheckStudentEnrollInCourseAsync(ClaimsPrincipal userprincipal, Course Course)
        {
            var user = await _userManager.GetUserAsync(userprincipal);

            var isfound = Course.Enrollments.FirstOrDefault(ct => ct.StudentId == user.Id);

            return isfound != null;
        }
    }
}
