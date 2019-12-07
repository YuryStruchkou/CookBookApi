using System.Linq;
using AutoMapper;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos.UserDetailsDtos;
using CookBook.Domain.ViewModels.UserViewModels;

namespace CookBook.Domain.Mappers
{
    public class UserDetailsProfile : Profile
    {
        public UserDetailsProfile()
        {
            CreateMap<UserProfile, UserDetailsDto>()
                .ForMember(u => u.UserName, src => src.MapFrom(u => u.ApplicationUser.UserName))
                .ForMember(u => u.Recipes, src => src.MapFrom(u => u.Recipes))
                .ForMember(u => u.AverageVote, src => src.MapFrom(u => u.Recipes
                    .SelectMany(r => r.Votes)
                    .DefaultIfEmpty(new Vote())
                    .Average(v => v.Value)));

            CreateMap<UpdateUserViewModel, UserProfile>();
        }
    }
}
