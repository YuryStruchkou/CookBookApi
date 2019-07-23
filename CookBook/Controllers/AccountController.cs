using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.CoreProject.Constants;
using CookBook.CoreProject.Interfaces;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ResultDtos.AccountDtos;
using CookBook.Domain.ViewModels.AccountViewModels;
using CookBook.Presentation.Filters;
using CookBook.Presentation.Helpers;
using CookBook.Presentation.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Presentation.Controllers
{
    [Route("api/account")]
    [ApiController]
    [ModelValidation]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtFactory _jwtFactory;
        private readonly RefreshTokenFactory _refreshTokenFactory;
        private readonly ICookieService _cookieService;
        private readonly IMapper _mapper;
        private static readonly string RefreshTokenCookieKey = "refresh_token"; 

        public AccountController(UserManager<ApplicationUser> userManager, IMapper mapper, JwtFactory jwtFactory, RefreshTokenFactory refreshTokenFactory, ICookieService cookieService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtFactory = jwtFactory;
            _refreshTokenFactory = refreshTokenFactory;
            _cookieService = cookieService;
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
            return await GenerateLoginResponse(user);
        }

        private async Task<ApplicationUser> VerifyUser(string userNameOrEmail, string password)
        {
            var userToVerify = await _userManager.FindByEmailAsync(userNameOrEmail) ??
                await _userManager.FindByNameAsync(userNameOrEmail);
            return userToVerify != null && await _userManager.CheckPasswordAsync(userToVerify, password)
                ? userToVerify
                : null;
        }

        private async Task<IActionResult> GenerateLoginResponse(ApplicationUser user)
        {
            var jwtToken = await GenerateJwtToken(user);
            var refreshToken = await GenerateAndSaveRefreshToken(user);
            _cookieService.WriteHttpOnlyCookie(RefreshTokenCookieKey, refreshToken.Token, refreshToken.ExpiryDate);
            return new OkObjectResult(new LoginResultDto
            {
                JwtToken = jwtToken.Token,
                ExpiryDate = jwtToken.ExpiryDate,
                UserName = user.UserName
            });
        }

        private async Task<JwtToken> GenerateJwtToken(ApplicationUser user)
        {
            var claims = await GetClaimsIdentity(user);
            return _jwtFactory.GenerateEncodedToken(claims);
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

        private async Task<RefreshToken> GenerateAndSaveRefreshToken(ApplicationUser user)
        {
            var token = _refreshTokenFactory.GenerateToken();
            user.RefreshTokens.Add(token);
            await _userManager.UpdateAsync(user);
            return token;
        }

        [HttpPost, Route("refresh"), AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null || !_cookieService.TryGetCookie(RefreshTokenCookieKey, out var token) || 
                user.RefreshTokens.All(t => t.Token != token))
            {
                return new UnauthorizedObjectResult(new ErrorDto((int)HttpStatusCode.Unauthorized, "Incorrect username or refresh token."));
            }
            var userToken = user.RefreshTokens.First(t => t.Token == token);
            user.RefreshTokens.Remove(userToken);
            await _userManager.UpdateAsync(user);
            if (userToken.ExpiryDate < DateTime.Now)
            {
                return new UnauthorizedObjectResult(new ErrorDto((int)HttpStatusCode.Unauthorized, "Token expired."));
            }
            return await GenerateLoginResponse(user);
        }
    }
}