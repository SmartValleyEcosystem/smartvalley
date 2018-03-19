using System.Collections.Generic;

namespace SmartValley.WebApi.WebApi
{
    public class PartialCollectionResponse<TItem> where TItem : new()
    {
        public IReadOnlyCollection<TItem> Items { get; }

        public int Offset { get; }

        public int Count { get; }

        public int TotalCount { get; }

        public PartialCollectionResponse(int offset, int count, int totalCount, IReadOnlyCollection<TItem> items)
        {
            Offset = offset;
            Count = count;
            TotalCount = totalCount;
            Items = items;
        }
    }
}