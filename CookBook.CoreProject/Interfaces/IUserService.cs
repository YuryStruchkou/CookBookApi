using System.Collections.Generic;
using System.Threading.Tasks;
using CookBook.Domain.Models;
using CookBook.Domain.ViewModels.UserViewModels;

namespace CookBook.CoreProject.Interfaces
{
    public interface IUserService
    {
        Task<UserProfile> GetAsync(int id);

        Task<UserProfile> UpdateAsync(UpdateUserViewModel model, int id);

        Task<List<UserProfile>> GetAllAsync();

        Task<UserProfile> BlockAsync(int id);

        Task<UserProfile> MuteAsync(int id);

        Task<UserProfile> RestoreAsync(int id);

        Task<UserProfile> MarkAsDeletedAsync(int id);
    }
}
