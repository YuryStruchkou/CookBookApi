using CookBook.BLL.Services;
using CookBook.DAL.Data;
using CookBook.Domain.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Testing.Mocking.BLL
{
    class CommentServiceMocking : BaseMocking<CommentService, CommentProfile>
    {
        private readonly ApplicationDbContext _context;

        public CommentServiceMocking(DbContextOptions<ApplicationDbContext> options)
        {
            _context = new ApplicationDbContext(options);
        }

        public override CommentService Setup()
        {
            var mapper = SetupMapper();
            return new CommentService(mapper, _context);
        }

        public void ClearContext()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
