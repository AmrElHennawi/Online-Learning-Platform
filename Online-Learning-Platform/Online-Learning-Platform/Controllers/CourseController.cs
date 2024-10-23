using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Learning_Platform.DataContext;
using Online_Learning_Platform.Interfaces;
using Online_Learning_Platform.Models;

namespace Online_Learning_Platform.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly DBContext _context;

        public CourseController(ICourseRepository courseRepository, UserManager<AppUser> userManager, DBContext context)
        {
            _courseRepository = courseRepository;
            _userManager = userManager;
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> GetAllCourses(string? searchString)
        {
            var courses = await _courseRepository.GetAllCoursesAsync(searchString);
            return View(courses);
        }


        [Authorize]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

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


        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UpdateCourse(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCourse(int id, Course updatedCourse)
        {
            if (id != updatedCourse.CourseId)
            {
                return BadRequest();
            }

            await _courseRepository.UpdateCourseAsync(updatedCourse);
            return RedirectToAction(nameof(GetAllCourses));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await _courseRepository.DeleteCourseAsync(id);
            return RedirectToAction(nameof(GetAllCourses));
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent(int id)
        {
            var studentId = _userManager.GetUserId(User);

            var enroll = new Enrollment()
            {
                CourseId = id,
                StudentId = studentId
            };

            await _courseRepository.EnrollStudentAsync(enroll);
            return RedirectToAction("GetAllCourses");
        }
    }
}