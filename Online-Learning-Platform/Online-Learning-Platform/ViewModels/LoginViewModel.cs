using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Online_Learning_Platform.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailAcive", controller: "Account")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
