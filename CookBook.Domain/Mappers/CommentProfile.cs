using AutoMapper;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos.CommentDtos;

namespace CookBook.Domain.Mappers
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDetailsDto>()
                .ForMember(c => c.UserId, src => src.MapFrom(c => c.User.UserId))
                .ForMember(c => c.UserName, src => src.MapFrom(c => c.User.ApplicationUser.UserName));
        }
    }
}
