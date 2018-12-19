using System.Collections.Generic;

namespace CookBook.Domain.Models
{
    public class RecipeStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
