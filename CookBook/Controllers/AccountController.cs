using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.CoreProject.Helpers;
using CookBook.Domain.Enums;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ViewModels;
using CookBook.Presentation.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Internal;

namespace CookBook.Presentation.Controllers
{
    [Route("api/account")]
    [ApiController]
    [ModelValidation]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpPost, Route("register"), AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegistrationViewModel model)
        {
            var user = _mapper.Map<RegistrationViewModel, ApplicationUser>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return new ConflictObjectResult(new ErrorDto((int) HttpStatusCode.Conflict, "User already exists."));
            }
            var resultDto = _mapper.Map<ApplicationUser, RegistrationResultDto>(user);
            return new OkObjectResult(resultDto);
        }
    }
}