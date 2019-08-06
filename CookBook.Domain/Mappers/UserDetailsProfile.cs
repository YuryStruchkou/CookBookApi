using AutoMapper;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos.UserDetailsDtos;

namespace CookBook.Domain.Mappers
{
    public class UserDetailsProfile : Profile
    {
        public UserDetailsProfile()
        {
            CreateMap<UserProfile, UserDetailsDto>()
                .ForMember(u => u.UserName, src => src.MapFrom(u => u.ApplicationUser.UserName))
                .ForMember(u => u.Recipes, src => src.MapFrom(u => u.Recipes));
        }
    }
}
