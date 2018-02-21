using System.Collections.Generic;

namespace SmartValley.Domain.Core
{
    public class PagingList<TItem> : List<TItem>
    {
        public PagingList(int totalCount, IEnumerable<TItem> items)
        {
            TotalCount = totalCount;
            AddRange(items);
        }

        public int TotalCount { get; set; }
    }
}