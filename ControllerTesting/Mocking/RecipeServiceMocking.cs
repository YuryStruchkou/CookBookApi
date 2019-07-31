using CookBook.BLL.Services;
using CookBook.DAL.Data;
using CookBook.Domain.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Testing.Mocking
{
    class RecipeServiceMocking : BaseMocking<RecipeService, RecipeProfile>
    {
        private readonly ApplicationDbContext _context;

        public RecipeServiceMocking(DbContextOptions<ApplicationDbContext> options)
        {
            _context = new ApplicationDbContext(options);
        }

        public override RecipeService Setup()
        {
            var mapper = SetupMapper();
            return new RecipeService(mapper, _context);
        }

        public void ClearContext()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
