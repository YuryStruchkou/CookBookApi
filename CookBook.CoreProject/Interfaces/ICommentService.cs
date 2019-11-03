using System.Threading.Tasks;
using CookBook.Domain.Models;
using CookBook.Domain.ViewModels.CommentViewModels;

namespace CookBook.CoreProject.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> AddAsync(CreateUpdateCommentViewModel model, int? userId);

        Task<Comment> GetAsync(int id);

        Task<Comment> UpdateAsync(CreateUpdateCommentViewModel model, int id);

        Task<bool> DeleteAsync(int id);
    }
}
