using System;
using System.Net;
using System.Threading.Tasks;
using CookBook.CoreProject.Helpers;
using CookBook.Domain.ResultDtos;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace CookBook.Presentation.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unhandled exception in a request");
                await HandleExceptionAsync(context);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var result = new ErrorDto(context.Response.StatusCode, "Internal server error.");
            await context.Response.WriteAsync(JsonConvertHelper.SerializeObjectCamelCase(result));
        }
    }
}
