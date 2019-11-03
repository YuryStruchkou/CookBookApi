using System.ComponentModel.DataAnnotations;

namespace CookBook.Domain.ViewModels.CommentViewModels
{
    public class CreateCommentViewModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int? RecipeId { get; set; }
    }
}
