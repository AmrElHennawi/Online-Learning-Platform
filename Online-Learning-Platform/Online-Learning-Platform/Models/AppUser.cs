using Microsoft.AspNetCore.Identity;

namespace Online_Learning_Platform.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
