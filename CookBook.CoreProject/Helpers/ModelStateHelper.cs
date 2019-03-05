using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CookBook.CoreProject.Helpers
{
    public static class ModelStateHelper
    {
        public static string[] GetErrors(this ModelStateDictionary modelState)
        {
            return modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(e => e.ErrorMessage)).ToArray();
        }

        public static void AddErrors(this ModelStateDictionary modelState, List<ValidationResult> errors)
        {
            foreach (var error in errors)
            {
                if (error.MemberNames.Any())
                {
                    foreach (var member in error.MemberNames)
                    {
                        modelState.AddModelError(member, error.ErrorMessage);
                    }
                }
                else
                {
                    modelState.AddModelError(String.Empty, error.ErrorMessage);
                }
            }
        }

        public static void Validate(this ModelStateDictionary modelState, object obj)
        {
            var errors = new List<ValidationResult>();
            Validator.TryValidateObject(obj, new ValidationContext(obj, null, null), errors, true);
            modelState.AddErrors(errors);
        }
    }
}
