namespace Online_Learning_Platform.Models
{
    public class Course
    {
        public int CourseId { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int Progress { get; set; }
        public ICollection<CourseTeacher> CourseTeachers { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
