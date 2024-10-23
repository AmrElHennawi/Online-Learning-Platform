using System.Collections.Generic;
using System.Threading.Tasks;
using Online_Learning_Platform.Models;

namespace Online_Learning_Platform.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync(string? searchString);
        Task<Course> GetCourseByIdAsync(int id);

        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int id);
        Task EnrollStudentAsync(Enrollment enrollment);
    }
}