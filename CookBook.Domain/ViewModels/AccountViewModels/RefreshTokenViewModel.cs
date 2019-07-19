using System.ComponentModel.DataAnnotations;

namespace CookBook.Domain.ViewModels.AccountViewModels
{
    public class RefreshTokenViewModel
    {
        [Required]
        public string UserName { get; set; }
    }
}
