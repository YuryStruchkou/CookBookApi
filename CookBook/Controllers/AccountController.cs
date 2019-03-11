using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.CoreProject.Constants;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ResultDtos.AccountDtos;
using CookBook.Domain.ViewModels.AccountViewModels;
using CookBook.Presentation.Filters;
using CookBook.Presentation.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CookBook.Presentation.Controllers
{
    [Route("api/account")]
    [ApiController]
    [ModelValidation]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtFactory _jwtFactory;
        private readonly IMapper _mapper;
        private const int TokenLifetime = 5;

        public AccountController(UserManager<ApplicationUser> userManager, IMapper mapper, JwtFactory jwtFactory)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtFactory = jwtFactory;
        }

        [HttpPost, Route("register"), AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegistrationViewModel model)
        {
            var user = _mapper.Map<RegistrationViewModel, ApplicationUser>(model);
            var createdResult = await _userManager.CreateAsync(user, model.Password);
            var addedToRoleResult = await _userManager.AddToRoleAsync(user, UserRoleNames.User);
            if (!createdResult.Succeeded || !addedToRoleResult.Succeeded)
            {
                return new ConflictObjectResult(new ErrorDto((int) HttpStatusCode.Conflict, "User already exists."));
            }
            var resultDto = _mapper.Map<ApplicationUser, RegistrationResultDto>(user);
            return new OkObjectResult(resultDto);
        }

        [HttpPost, Route("login"), AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await VerifyUser(model.UserNameOrEmail, model.Password);
            if (user == null)
            {
                return new BadRequestObjectResult(new ErrorDto((int) HttpStatusCode.BadRequest, "Incorrect username and/or password."));
            }
            var claims = await GetClaimsIdentity(user);
            return new OkObjectResult(new LoginResultDto
            {
                JwtToken = _jwtFactory.GenerateEncodedToken(claims, TokenLifetime),
                UserName = user.UserName
            });
        }

        private async Task<ApplicationUser> VerifyUser(string userNameOrEmail, string password)
        {
            var userToVerify = await _userManager.FindByEmailAsync(userNameOrEmail) ??
                await _userManager.FindByNameAsync(userNameOrEmail);
            return userToVerify != null && await _userManager.CheckPasswordAsync(userToVerify, password)
                ? userToVerify
                : null;
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(ApplicationUser user)
        {
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role ?? string.Empty)
            };
            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}