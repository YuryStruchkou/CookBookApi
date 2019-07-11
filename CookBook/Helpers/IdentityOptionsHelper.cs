using Microsoft.AspNetCore.Identity;

namespace CookBook.Presentation.Helpers
{
    public static class IdentityOptionsHelper
    {
        public static void ConfigureIdentityOptions(this IdentityOptions options)
        {
            options.User.RequireUniqueEmail = true;
        }
    }
}
