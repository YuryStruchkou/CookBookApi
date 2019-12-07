namespace CookBook.Domain.ResultDtos.AccountDtos
{
    public class RegistrationResultDto
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public bool IsMuted { get; set; }

        public string UserStatus { get; set; }

        public string ImagePublicId { get; set; }
    }
}
