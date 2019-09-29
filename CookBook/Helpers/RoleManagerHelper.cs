using System.Threading.Tasks;
using CookBook.CoreProject.Constants;
using Microsoft.AspNetCore.Identity;

namespace CookBook.Presentation.Helpers
{
    public static class RoleManagerHelper
    {
        public static async Task SeedRoles(this RoleManager<IdentityRole<int>> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(UserRoleNames.User))
            {
                await roleManager.CreateAsync(new IdentityRole<int> {Name = UserRoleNames.User});
            }
            if (!await roleManager.RoleExistsAsync(UserRoleNames.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole<int> { Name = UserRoleNames.Admin });
            }
        }
    }
}
