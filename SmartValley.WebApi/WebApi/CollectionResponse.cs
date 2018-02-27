using System.Collections.Generic;

namespace SmartValley.WebApi.WebApi
{
    public class CollectionResponse<TItem>
    {
        public IReadOnlyCollection<TItem> Items { get; set; }
        public int TotalCount { get; set; }
    }
}