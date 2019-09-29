using CookBook.BLL.Services;
using CookBook.DAL.Data;
using CookBook.Domain.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Testing.Mocking
{
    class RecipeServiceMocking : BaseMocking<RecipeService, RecipeProfile>
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public RecipeServiceMocking(DbContextOptions<ApplicationDbContext> options)
        {
            _options = options;
        }

        public override RecipeService Setup()
        {
            var mapper = SetupMapper();
            return new RecipeService(mapper, new ApplicationDbContext(_options));
        }
    }
}
