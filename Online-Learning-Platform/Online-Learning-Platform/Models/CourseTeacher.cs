namespace Online_Learning_Platform.Models
{
    public class CourseTeacher
    {
        public int CourseId { get; set; }
        public string TeacherId { get; set; }
        public Course Course { get; set; }
        public AppUser Teacher { get; set; }
    }
}
