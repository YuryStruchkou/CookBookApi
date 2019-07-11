using System;
using System.Reflection;
using CookBook.Domain.ResultDtos;
using CookBook.Presentation.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Testing.Helpers
{
    public static class AttributeHelper
    {
        public static bool IsModelValidationApplied(Type type, string methodName)
        {
            return (type.GetCustomAttribute(typeof(ModelValidationAttribute)) ??
                   type.GetMethod(methodName).GetCustomAttribute(typeof(ModelValidationAttribute))) != null;
        }

        public static ErrorDto ExecuteModelValidation(ActionExecutingContext context)
        {
            new ModelValidationAttribute().OnActionExecuting(context);
            var json = (BadRequestObjectResult)context.Result;
            return (ErrorDto)json.Value;
        }
    }
}
