using CookBook.CoreProject.Interfaces;
using CookBook.Domain.Mappers;
using CookBook.Domain.Models;
using CookBook.Domain.ViewModels.CommentViewModels;
using CookBook.Presentation.Controllers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Testing.Mocking.Presentation
{
    class CommentControllerMocking : BaseMocking<CommentController, CommentProfile>
    {
        private static readonly int DefaultUserId = 1;

        public Mock<ICommentService> CommentServiceMock { get; } = new Mock<ICommentService>();

        public Mock<UserManager<ApplicationUser>> UserManagerMock { get; } = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object,
            null, null, null, null, null, null, null, null);

        public override CommentController Setup()
        {
            var mapper = SetupMapper();
            var commentService = MockCommentService().Object;
            var userManager = MockUserManager().Object;
            var controller = new CommentController(mapper, commentService, userManager);
            return controller;
        }

        private Mock<ICommentService> MockCommentService()
        {
            CommentServiceMock.Setup(s => s.AddAsync(It.IsAny<CreateCommentViewModel>(), It.IsAny<int>()))
                .ReturnsAsync((CreateCommentViewModel m, int id) => new Comment
                {
                    UserId = id, 
                    Id = 1,
                    Content = m.Content
                });

            CommentServiceMock.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => id > 0
                ? new Comment
                {
                    Id = id, 
                    UserId = id, 
                    Content = MockConstants.DefaultComment.Content
                }
                : null);

            CommentServiceMock.Setup(s => s.UpdateAsync(It.IsAny<UpdateCommentViewModel>(), It.IsAny<int>()))
                .ReturnsAsync((UpdateCommentViewModel m, int id) => new Comment
                {
                    Id = id,
                    Content = m.Content
                });

            CommentServiceMock.Setup(s => s.DeleteAsync(It.IsAny<int>())).ReturnsAsync((int id) => id > 0);

            return CommentServiceMock;
        }

        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            UserManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser { Id = DefaultUserId });
            return UserManagerMock;
        }
    }
}
