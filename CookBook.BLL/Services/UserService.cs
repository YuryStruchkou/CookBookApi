using System.Threading.Tasks;
using CookBook.CoreProject.Interfaces;
using CookBook.DAL.Data;
using CookBook.Domain.Models;

namespace CookBook.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserProfile> GetAsync(int id)
        {
            var user = await _dbContext.UserProfiles.FindAsync(id);
            return user;
        }
    }
}
