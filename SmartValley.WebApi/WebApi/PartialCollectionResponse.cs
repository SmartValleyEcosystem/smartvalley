using System.Collections.Generic;

namespace SmartValley.WebApi.WebApi
{
    public class PartialCollectionResponse<TItem> where TItem : class
    {
        public IReadOnlyCollection<TItem> Items { get; }

        public int Offset { get; }

        public int Count { get; }

        public int TotalCount { get; }

        public PartialCollectionResponse(IReadOnlyCollection<TItem> items, int offset, int totalCount)
        {
            Offset = offset;
            Count = items.Count;
            TotalCount = totalCount;
            Items = items;
        }
    }
}