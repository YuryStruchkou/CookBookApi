namespace CookBook.Domain.ResultDtos.RecipeDtos
{
    public class RecipeBriefDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePublicId { get; set; }

        public double AverageVote { get; set; }

        public string RecipeStatus { get; set; }
    }
}
