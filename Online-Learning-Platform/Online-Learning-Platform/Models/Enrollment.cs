namespace Online_Learning_Platform.Models
{
    public class Enrollment
    {
        public int CourseId { get; set; }

        public string StudentId { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        public Course Course { get; set; }

        public AppUser Student { get; set; }

    }
}
