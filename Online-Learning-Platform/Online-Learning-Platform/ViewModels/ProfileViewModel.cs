using Online_Learning_Platform.Models;

namespace Online_Learning_Platform.ViewModels
{
	public class ProfileViewModel
	{
		public AppUser User { get; set; }
		public string Role { get; set; }
		public List<Course> Courses { get; set; }
	}
}
