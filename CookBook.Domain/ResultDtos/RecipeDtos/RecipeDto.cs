using System;
using System.Collections.Generic;

namespace CookBook.Domain.ResultDtos.RecipeDtos
{
    public class RecipeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public List<string> Tags { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? EditDate { get; set; }

        public DateTime? DeleteDate { get; set; }

        public int TotalVotes { get; set; }
        
        public double AverageVote { get; set; }

        public int? UserVote { get; set; }

        public int RecipeStatusId { get; set; }
    }
}
