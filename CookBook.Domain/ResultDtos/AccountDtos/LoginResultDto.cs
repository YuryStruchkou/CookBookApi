using System;

namespace CookBook.Domain.ResultDtos.AccountDtos
{
    public class LoginResultDto
    {
        public string JwtToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string UserName { get; set; }
    }
}
