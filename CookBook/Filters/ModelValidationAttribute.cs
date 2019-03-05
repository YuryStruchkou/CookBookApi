using System.Net;
using CookBook.CoreProject.Helpers;
using CookBook.Domain.ResultDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CookBook.Presentation.Filters
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ErrorDto((int)HttpStatusCode.BadRequest, context.ModelState.GetErrors()));
            }
        }
    }
}
