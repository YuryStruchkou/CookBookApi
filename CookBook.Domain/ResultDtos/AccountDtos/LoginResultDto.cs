namespace CookBook.Domain.ResultDtos.AccountDtos
{
    public class LoginResultDto
    {
        public string JwtToken { get; set; }
        public string UserName { get; set; }
    }
}
