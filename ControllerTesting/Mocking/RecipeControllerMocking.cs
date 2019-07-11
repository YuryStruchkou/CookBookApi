using CookBook.Domain.Mappers;
using CookBook.Presentation.Controllers;

namespace Testing.Mocking
{
    class RecipeControllerMocking : BaseMocking<RecipeController, RecipeProfile>
    {
        public override RecipeController Setup()
        {
            var mapper = SetupMapper();
            return new RecipeController(mapper);
        }
    }
}
