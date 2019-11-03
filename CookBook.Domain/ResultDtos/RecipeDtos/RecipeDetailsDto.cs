using System;
using System.Collections.Generic;
using CookBook.Domain.ResultDtos.CommentDtos;

namespace CookBook.Domain.ResultDtos.RecipeDtos
{
    public class RecipeDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public List<string> Tags { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? EditDate { get; set; }

        public DateTime? DeleteDate { get; set; }

        public RecipeVoteDto RecipeVoteData { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }

        public string RecipeStatus { get; set; }

        public List<CommentDetailsDto> Comments { get; set; }
    }
}
