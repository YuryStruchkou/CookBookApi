using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Testing.Mocking
{
    abstract class BaseMocking<T, TProfile> where TProfile : Profile, new()
    {
        public abstract T Setup();

        protected IMapper SetupMapper()
        {
            var config = new MapperConfiguration(c => c.AddProfile<TProfile>());
            return config.CreateMapper();
        }

        public ActionExecutingContext SetupContext(ControllerBase controller)
        {
            return new ActionExecutingContext(new ControllerContext(new ActionContext(new Mock<HttpContext>().Object, new RouteData(),
                new ControllerActionDescriptor())), new List<IFilterMetadata>(), new Dictionary<string, object>(), controller);
        }
    }
}
