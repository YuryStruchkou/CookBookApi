using System.Linq;
using CookBook.Domain.Enums;
using CookBook.Domain.Models;

namespace CookBook.Domain.Helpers
{
    public static class DomainHelper
    {
        public static double GetAverageVote(this Recipe recipe)
        {
            return recipe.Votes.DefaultIfEmpty(new Vote()).Average(vote => vote.Value);
        }

        public static bool IsBlockedOrDeleted(this ApplicationUser user)
        {
            return user.UserProfile.UserStatus == UserStatus.Blocked || user.UserProfile.UserStatus == UserStatus.Deleted;
        }
    }
}
