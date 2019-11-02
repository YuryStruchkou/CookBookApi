using System.Collections.Generic;
using Nest;

namespace CookBook.Domain.ElasticSearch
{
    [ElasticsearchType(RelationName = "recipe")]
    public class RecipeDocument
    {
        public int Id { get; set; }

        [Text]
        public string Name { get; set; }

        [Text]
        public string Description { get; set; }

        [Text]
        public string Content { get; set; }

        [Text]
        public List<string> Tags { get; set; }
    }
}
