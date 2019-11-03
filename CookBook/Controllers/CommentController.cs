using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.CoreProject.Constants;
using CookBook.CoreProject.Helpers;
using CookBook.CoreProject.Interfaces;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ResultDtos.CommentDtos;
using CookBook.Domain.ViewModels.CommentViewModels;
using CookBook.Presentation.Filters;
using CookBook.Presentation.ObjectResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Presentation.Controllers
{
    [Route("api/comment")]
    [ApiController]
    [ModelValidation]
    public class CommentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(IMapper mapper, ICommentService commentService, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _commentService = commentService;
            _userManager = userManager;
        }

        [HttpPost, Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateComment([FromBody] CreateUpdateCommentViewModel model)
        {
            var userId = await _userManager.GetCurrentUserIdAsync(User);
            var comment = await _commentService.AddAsync(model, userId);
            var result = _mapper.Map<Comment, CommentDetailsDto>(comment);
            return new OkObjectResult(result);
        }

        [HttpGet, Route("{id}"), AllowAnonymous]
        public async Task<IActionResult> GetComment([FromRoute] int id)
        {
            var recipe = await _commentService.GetAsync(id);
            var result = _mapper.Map<Comment, CommentDetailsDto>(recipe);
            return result != null
                ? (IActionResult)new OkObjectResult(result)
                : new NotFoundObjectResult(new ErrorDto((int)HttpStatusCode.NotFound, "Comment not found."));
        }

        [HttpPut, Route("{id}"), Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateComment([FromBody] CreateUpdateCommentViewModel model, [FromRoute] int id)
        {
            var comment = await _commentService.GetAsync(id);
            if (comment == null)
            {
                return new NotFoundObjectResult(new ErrorDto((int)HttpStatusCode.NotFound, "Comment not found."));
            }
            if (!User.IsInRole(UserRoleNames.Admin) && comment.UserId != await _userManager.GetCurrentUserIdAsync(User))
            {
                return new ForbiddenObjectResult(new ErrorDto((int)HttpStatusCode.Forbidden, "User id does not match."));
            }
            comment = await _commentService.UpdateAsync(model, comment.Id);
            var result = _mapper.Map<Comment, CommentDetailsDto>(comment);
            return new OkObjectResult(result);
        }

        [HttpDelete, Route("{id}"), Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            var comment = await _commentService.GetAsync(id);
            if (comment == null)
            {
                return new NotFoundObjectResult(new ErrorDto((int)HttpStatusCode.NotFound, "Comment not found."));
            }
            if (!User.IsInRole(UserRoleNames.Admin) && comment.UserId != await _userManager.GetCurrentUserIdAsync(User))
            {
                return new ForbiddenObjectResult(new ErrorDto((int)HttpStatusCode.Forbidden, "User id does not match."));
            }
            await _commentService.DeleteAsync(id);
            return new NoContentResult();
        }
    }
}