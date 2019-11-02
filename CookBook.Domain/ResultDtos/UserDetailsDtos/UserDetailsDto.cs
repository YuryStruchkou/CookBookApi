using System.Collections.Generic;
using CookBook.Domain.ResultDtos.RecipeDtos;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace CookBook.Domain.ResultDtos.UserDetailsDtos
{
    public class UserDetailsDto
    {
        public string UserName { get; set; }

        public string AvatarUrl { get; set; }

        public double AverageVote { get; set; }

        public List<RecipeBriefDto> Recipes { get; set; }
    }
}
