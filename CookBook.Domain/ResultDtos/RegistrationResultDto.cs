namespace CookBook.Domain.ResultDtos
{
    public class RegistrationResultDto
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public bool IsMuted { get; set; }

        public int UserStatusId { get; set; }

        public string AvatarUrl { get; set; }
    }
}
