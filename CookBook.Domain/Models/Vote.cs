namespace CookBook.Domain.Models
{
    public class Vote
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public int? UserId { get; set; }

        public virtual UserProfile User { get; set; }

        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
