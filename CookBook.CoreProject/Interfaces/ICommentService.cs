using System.Threading.Tasks;
using CookBook.Domain.Models;
using CookBook.Domain.ViewModels.CommentViewModels;

namespace CookBook.CoreProject.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> AddAsync(CreateCommentViewModel model, int? userId);

        Task<Comment> GetAsync(int id);

        Task<Comment> UpdateAsync(UpdateCommentViewModel model, int id);

        Task<bool> DeleteAsync(int id);
    }
}
