using System.Collections.Generic;
using System.Linq;
using CookBook.Domain.Models;

namespace CookBook.Queries.QueryableExtensions
{
    public static class QueryableHelper
    {
        public static IEnumerable<Tag> GetAttachableTags(this IQueryable<Tag> tags, IEnumerable<string> tagsToAttach)
        {
            var dbTags = tags.Where(t => tagsToAttach.Contains(t.Content)).ToList();
            var dbTagNames = dbTags.Select(t => t.Content).ToList();
            var newTags = tagsToAttach.Except(dbTagNames).Select(t => new Tag { Content = t }).ToList();
            dbTags.AddRange(newTags);
            return dbTags;
        }
    }
}
