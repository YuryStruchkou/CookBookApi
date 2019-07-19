using Microsoft.AspNetCore.Http;

namespace CookBook.Presentation.Helpers
{
    public static class HttpContextHelper
    {
        public static void AppendHttpOnlyCookie(this HttpContext context, string key, string value)
        {
            context.Response.Cookies.Append(key, value, new CookieOptions { HttpOnly = true });
        }

        public static bool TryGetCookie(this HttpContext context, string key, out string value)
        {
            return context.Request.Cookies.TryGetValue(key, out value);
        }
    }
}
