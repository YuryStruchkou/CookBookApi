using System.ComponentModel.DataAnnotations;

namespace CookBook.Domain.ViewModels.CommentViewModels
{
    public class UpdateCommentViewModel
    {
        [Required]
        public string Content { get; set; }
    }
}
