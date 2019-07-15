using Microsoft.AspNetCore.Mvc;

namespace CookBook.Presentation.ObjectResults
{
    public class ForbiddenObjectResult : ObjectResult
    {
        private const int DefaultStatusCode = 403;

        public ForbiddenObjectResult(object value)
            : base(value)
        {
            this.StatusCode = DefaultStatusCode;
        }
    }
}
