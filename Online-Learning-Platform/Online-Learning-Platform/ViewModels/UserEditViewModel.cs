using Microsoft.AspNetCore.Mvc.Rendering;
using Online_Learning_Platform.Models;

namespace Online_Learning_Platform.ViewModels
{
    public class UserEditViewModel
    {
        public AppUser User { get; set; }

        public string Role { get; set; }

        public IEnumerable<SelectListItem> SelectedRoles { get; set; }
    }
}
