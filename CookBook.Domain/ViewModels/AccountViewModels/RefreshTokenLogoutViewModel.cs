using System.ComponentModel.DataAnnotations;

namespace CookBook.Domain.ViewModels.AccountViewModels
{
    public class RefreshTokenLogoutViewModel
    {
        [Required]
        public string UserName { get; set; }
    }
}
