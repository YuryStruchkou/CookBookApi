using System.Security.Claims;
using System.Threading.Tasks;
using CookBook.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace CookBook.CoreProject.Helpers
{
    public static class UserManagerHelper
    {
        public static async Task<int?> GetCurrentUserIdAsync(this UserManager<ApplicationUser> userManager, ClaimsPrincipal user)
        {
            return (await userManager.GetCurrentUserAsync(user))?.Id;
        }

        public static async Task<ApplicationUser> GetCurrentUserAsync(this UserManager<ApplicationUser> userManager, ClaimsPrincipal user)
        {
            var username = userManager.GetUserName(user) ?? "";
            return await userManager.FindByNameAsync(username);
        }
    }
}
