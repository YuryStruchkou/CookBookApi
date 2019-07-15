using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CookBook.Domain.ViewModels.RecipeViewModels
{
    public class CreateUpdateRecipeViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        public List<string> Tags { get; set; }
    }
}
