using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.WebApi
{
    public class CollectionResponse<TItem>
    {
        public IReadOnlyCollection<TItem> Items { get; set; }
    }
}