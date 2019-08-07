using System.Threading.Tasks;
using CookBook.Domain.Models;

namespace CookBook.CoreProject.Interfaces
{
    public interface IUserService
    {
        Task<UserProfile> GetAsync(int id);
    }
}
