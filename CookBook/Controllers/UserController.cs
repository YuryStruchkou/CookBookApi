using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.CoreProject.Constants;
using CookBook.CoreProject.Helpers;
using CookBook.CoreProject.Interfaces;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ResultDtos.UserDetailsDtos;
using CookBook.Domain.ViewModels.UserViewModels;
using CookBook.Presentation.ObjectResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Presentation.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserService userService, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var user = await _userService.GetAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult(new ErrorDto((int)HttpStatusCode.NotFound, "User does not exist."));
            }
            var result = _mapper.Map<UserProfile, UserDetailsDto>(user);
            return new OkObjectResult(result);
        }

        [HttpPatch("{id}"), Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateUserViewModel model)
        {
            var user = await _userService.GetAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult(new ErrorDto((int)HttpStatusCode.NotFound, "User not found."));
            }
            if (!User.IsInRole(UserRoleNames.Admin) && user.UserId != await _userManager.GetCurrentUserIdAsync(User))
            {
                return new ForbiddenObjectResult(new ErrorDto((int)HttpStatusCode.Forbidden, "User id does not match."));
            }
            user = await _userService.UpdateAsync(model, user.UserId);
            var result = _mapper.Map<UserProfile, UserDetailsDto>(user);
            return new OkObjectResult(result);
        }

        [HttpGet(""), Authorize(AuthenticationSchemes = "Bearer", Policy = Policy.ApiAdmin)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            var result = users.Select(_mapper.Map<UserProfile, UserDetailsDto>);
            return new OkObjectResult(result);
        }

        [HttpPatch("{id}/block"), Authorize(AuthenticationSchemes = "Bearer", Policy = Policy.ApiAdmin)]
        public async Task<IActionResult> BlockUser([FromRoute] int id)
        {
            var currentUserId = await _userManager.GetCurrentUserIdAsync(User);
            if (currentUserId == id)
            {
                return new ForbiddenObjectResult(new ErrorDto((int) HttpStatusCode.Forbidden, "Operation cannot be performed for current user."));
            }
            var user = await _userService.BlockAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult(new ErrorDto((int)HttpStatusCode.NotFound, "User does not exist."));
            }
            var result = _mapper.Map<UserProfile, UserDetailsDto>(user);
            return new OkObjectResult(result);
        }

        [HttpPatch("{id}/mute"), Authorize(AuthenticationSchemes = "Bearer", Policy = Policy.ApiAdmin)]
        public async Task<IActionResult> MuteUser([FromRoute] int id)
        {
            var currentUserId = await _userManager.GetCurrentUserIdAsync(User);
            if (currentUserId == id)
            {
                return new ForbiddenObjectResult(new ErrorDto((int)HttpStatusCode.Forbidden, "Operation cannot be performed for current user."));
            }
            var user = await _userService.MuteAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult(new ErrorDto((int)HttpStatusCode.NotFound, "User does not exist."));
            }
            var result = _mapper.Map<UserProfile, UserDetailsDto>(user);
            return new OkObjectResult(result);
        }

        [HttpPatch("{id}/restore"), Authorize(AuthenticationSchemes = "Bearer", Policy = Policy.ApiAdmin)]
        public async Task<IActionResult> RestoreUser([FromRoute] int id)
        {
            var currentUserId = await _userManager.GetCurrentUserIdAsync(User);
            if (currentUserId == id)
            {
                return new ForbiddenObjectResult(new ErrorDto((int)HttpStatusCode.Forbidden, "Operation cannot be performed for current user."));
            }
            var user = await _userService.RestoreAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult(new ErrorDto((int)HttpStatusCode.NotFound, "User does not exist."));
            }
            var result = _mapper.Map<UserProfile, UserDetailsDto>(user);
            return new OkObjectResult(result);
        }

        [HttpDelete("{id}"), Authorize(AuthenticationSchemes = "Bearer", Policy = Policy.ApiAdmin)]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var currentUserId = await _userManager.GetCurrentUserIdAsync(User);
            if (currentUserId == id)
            {
                return new ForbiddenObjectResult(new ErrorDto((int)HttpStatusCode.Forbidden, "Operation cannot be performed for current user."));
            }
            var user = await _userService.MarkAsDeletedAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult(new ErrorDto((int)HttpStatusCode.NotFound, "User does not exist."));
            }
            var result = _mapper.Map<UserProfile, UserDetailsDto>(user);
            return new OkObjectResult(result);
        }
    }
}