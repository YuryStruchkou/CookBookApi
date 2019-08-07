using CookBook.BLL.Services;
using CookBook.DAL.Data;
using CookBook.Domain.Mappers;
using CookBook.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Testing.Mocking
{
    class UserServiceMocking : BaseMocking<UserService, UserDetailsProfile>
    {
        private readonly ApplicationDbContext _context;

        public UserServiceMocking(DbContextOptions<ApplicationDbContext> options)
        {
            _context = new ApplicationDbContext(options);
        }

        public override UserService Setup()
        {
            PopulateContext();
            return new UserService(_context);
        }

        private void PopulateContext()
        {
            _context.UserProfiles.Add(new UserProfile { UserId = 1 });
            _context.SaveChanges();
        }

        public void ClearContext()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
