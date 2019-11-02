using CookBook.BLL.Services;
using CookBook.CoreProject.Interfaces;
using CookBook.DAL.Data;
using CookBook.Domain.Mappers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Testing.Mocking.BLL
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
            return new RecipeService(mapper, _context, new Mock<ISearchService>().Object);
        }

        public void ClearContext()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
