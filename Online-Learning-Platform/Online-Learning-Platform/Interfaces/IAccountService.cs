using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Online_Learning_Platform.ViewModels;

namespace Online_Learning_Platform.Interfaces
{
    public interface IAccountService
    {
        Task<ProfileViewModel> GetProfileAsync(string id, ClaimsPrincipal User);
    }
}