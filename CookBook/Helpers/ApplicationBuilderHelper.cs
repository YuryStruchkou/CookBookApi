using CookBook.Presentation.Middleware;
using Microsoft.AspNetCore.Builder;

namespace CookBook.Presentation.Helpers
{
    public static class ApplicationBuilderHelper
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
