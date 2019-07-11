using System;
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
            CreateMap<CreateRecipeViewModel, Recipe>()
                .ForMember(r => r.RecipeStatus, src => src.MapFrom(vm => RecipeStatus.Active))
                .ForMember(r => r.CreationDate, src => src.MapFrom(vm => DateTime.Now));
            CreateMap<Recipe, RecipeDto>();
        }
    }
}
