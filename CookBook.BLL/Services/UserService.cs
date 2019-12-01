using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBook.CoreProject.Interfaces;
using CookBook.DAL.Data;
using CookBook.Domain.Enums;
using CookBook.Domain.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<UserProfile>> GetAllAsync()
        {
            return await _dbContext.UserProfiles.OrderBy(u => u.ApplicationUser.UserName).ToListAsync();
        }

        public async Task<UserProfile> BlockAsync(int id)
        {
            var user = await _dbContext.UserProfiles.FindAsync(id);
            if (user == null) return null;
            user.UserStatus = UserStatus.Blocked;
            _dbContext.UserProfiles.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserProfile> MuteAsync(int id)
        {
            var user = await _dbContext.UserProfiles.FindAsync(id);
            if (user == null) return null;
            user.IsMuted = true;
            _dbContext.UserProfiles.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserProfile> RestoreAsync(int id)
        {
            var user = await _dbContext.UserProfiles.FindAsync(id);
            if (user == null) return null;
            user.UserStatus = UserStatus.Active;
            user.IsMuted = false;
            _dbContext.UserProfiles.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserProfile> MarkAsDeletedAsync(int id)
        {
            var user = await _dbContext.UserProfiles.FindAsync(id);
            if (user == null) return null;
            user.UserStatus = UserStatus.Deleted;
            _dbContext.UserProfiles.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
