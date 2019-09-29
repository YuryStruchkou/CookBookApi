using System.Collections.Generic;

namespace CookBook.Domain.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public virtual ICollection<RecipeTag> RecipeTags { get; set; } = new HashSet<RecipeTag>();
    }
}
