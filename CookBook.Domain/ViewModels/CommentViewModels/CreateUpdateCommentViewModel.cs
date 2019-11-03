using System.ComponentModel.DataAnnotations;

namespace CookBook.Domain.ViewModels.CommentViewModels
{
    public class CreateUpdateCommentViewModel
    {
        [Required]
        public string Content { get; set; }
    }
}
