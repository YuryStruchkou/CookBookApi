using System;
using System.Reflection;
using CookBook.Presentation.Filters;

namespace ControllerTesting.Helpers
{
    public static class AttributeHelper
    {
        public static bool IsModelValidationApplied(Type type, string methodName)
        {
            return (type.GetCustomAttribute(typeof(ModelValidationAttribute)) ??
                   type.GetMethod(methodName).GetCustomAttribute(typeof(ModelValidationAttribute))) != null;
        }
    }
}
