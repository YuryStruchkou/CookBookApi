using AutoMapper;
using CookBook.Domain.Enums;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ViewModels;

namespace CookBook.Domain.Mappers
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<RegistrationViewModel, ApplicationUser>()
                .ForMember(u => u.UserProfile,
                    src => src.MapFrom(vm => new UserProfile
                    {
                        IsMuted = false,
                        UserStatusId = (int) UserStatuses.Active
                    }));

            CreateMap<UserProfile, RegistrationResultDto>();
            CreateMap<ApplicationUser, RegistrationResultDto>()
                .ForMember(dto => dto.IsMuted, src => src.MapFrom(u => u.UserProfile.IsMuted))
                .ForMember(dto => dto.UserStatusId, src => src.MapFrom(u => u.UserProfile.UserStatusId))
                .ForMember(dto => dto.AvatarUrl, src => src.MapFrom(u => u.UserProfile.AvatarUrl));
        }
    }
}
