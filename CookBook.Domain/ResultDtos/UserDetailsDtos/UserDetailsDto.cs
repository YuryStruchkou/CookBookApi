using System.Collections.Generic;
using CookBook.Domain.Enums;
using CookBook.Domain.ResultDtos.RecipeDtos;

namespace CookBook.Domain.ResultDtos.UserDetailsDtos
{
    public class UserDetailsDto
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string ImagePublicId { get; set; }

        public double AverageVote { get; set; }

        public string UserStatus { get; set; }

        public bool IsMuted { get; set; }

        public List<RecipeBriefDto> Recipes { get; set; }
    }
}
