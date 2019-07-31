using System.Linq;
using AutoMapper;
using CookBook.Domain.Enums;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos.RecipeDtos;
using CookBook.Domain.ViewModels.RecipeViewModels;

namespace CookBook.Domain.Mappers
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<CreateUpdateRecipeViewModel, Recipe>()
                .ForMember(r => r.RecipeStatus, src => src.MapFrom(vm => RecipeStatus.Active));
            CreateMap<Recipe, RecipeVoteDto>()
                .ForMember(v => v.TotalVotes, src => src.MapFrom(r => r.Votes.Count))
                .ForMember(v => v.AverageVote, src => src.MapFrom(r => r.Votes.DefaultIfEmpty(new Vote()).Average(vote => vote.Value)));
            CreateMap<Recipe, RecipeDto>()
                .ForMember(r => r.Tags, src => src.MapFrom(r => r.RecipeTags.Select(rt => rt.Tag.Content)))
                .ForMember(r => r.UserName, src => src.MapFrom(r => r.User.ApplicationUser.UserName))
                .ForMember(r => r.RecipeVoteData, src => src.MapFrom(r => r));
        }
    }
}
