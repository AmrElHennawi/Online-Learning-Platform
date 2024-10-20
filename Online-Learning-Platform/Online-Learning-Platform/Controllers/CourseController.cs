using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Learning_Platform.DataContext;
using Online_Learning_Platform.Models;
using System.Collections.Immutable;

namespace Online_Learning_Platform.Controllers
{
    public class CourseController : Controller
    {
        private readonly DBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CourseController(DBContext dbContext, UserManager<AppUser> userManager,
                                RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _context = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: Get all courses
        [Authorize]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.CourseTeachers)
                .Include(c => c.Enrollments)
                .ToListAsync();
            return View(courses); // Assuming you have a view that takes a list of courses.
        }

        // GET: Get a course by ID
        [Authorize]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.CourseTeachers)
                    .ThenInclude(ct => ct.Teacher)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Display form to create a new course
        [Authorize(Roles = "Teacher")]
        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(Course course)
        {

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var courseteacher = new CourseTeacher()
            {
                CourseId = course.CourseId,
                TeacherId = _userManager.GetUserId(User)
            };

            _context.CourseTeachers.Add(courseteacher);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetAllCourses");

        }


        // GET: Display form to update an existing course
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UpdateCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course); // Assuming you have a form in the view for editing a course.
        }

        // POST: Update an existing course
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCourse(int id, Course updatedCourse)
        {
            if (id != updatedCourse.CourseId)
            {
                return BadRequest();
            }

            try
            {
                _context.Update(updatedCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Courses.Any(c => c.CourseId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(GetAllCourses));

        }

        // POST: Delete a course
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetAllCourses));
        }

        // POST: Delete a course
        [HttpPost]
        public async Task<IActionResult> EnrolleStudent(int id)
        {
            var Studentid = _userManager.GetUserId(User);

            var enroll = new Enrollment()
            {
                CourseId = id,
                StudentId = Studentid
            };

            _context.Enrollments.Add(enroll);
            await _context.SaveChangesAsync();

            return RedirectToAction("getallcourses");
        }
    }
}
