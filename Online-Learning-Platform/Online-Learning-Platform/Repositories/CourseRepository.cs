using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Learning_Platform.DataContext;
using Online_Learning_Platform.Interfaces;
using Online_Learning_Platform.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Learning_Platform.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DBContext _context;

        public CourseRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync(string? searchString)
        {
                var courses = _context.Courses.Include(c => c.CourseTeachers).Include(c => c.Enrollments).AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    courses = courses.Where(c => c.Title.Contains(searchString) || c.Description.Contains(searchString));
                }

                return await courses.ToListAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.CourseTeachers)
                .ThenInclude(ct => ct.Teacher)
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.CourseId == id); ;
        }

        public async Task UpdateCourseAsync(Course course)
        {
            _context.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EnrollStudentAsync(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }
    }
}
