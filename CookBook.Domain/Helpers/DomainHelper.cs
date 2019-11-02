using System.Linq;
using CookBook.Domain.Models;

namespace CookBook.Domain.Helpers
{
    public static class DomainHelper
    {
        public static double GetAverageVote(this Recipe recipe)
        {
            return recipe.Votes.DefaultIfEmpty(new Vote()).Average(vote => vote.Value);
        }
    }
}
