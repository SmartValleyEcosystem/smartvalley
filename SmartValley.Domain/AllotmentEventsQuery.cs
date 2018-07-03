using System;
using System.Collections.Generic;

namespace SmartValley.Domain
{
    public class AllotmentEventsQuery : CollectionQuery
    {
        public AllotmentEventsQuery(IReadOnlyCollection<AllotmentEventStatus> allotmentEventStatuses, IReadOnlyCollection<long> ids, int offset, int count)
            : base(offset, count)
        {
            AllotmentEventStatuses = allotmentEventStatuses;
            Ids = ids;
        }

        public IReadOnlyCollection<AllotmentEventStatus> AllotmentEventStatuses { get; }

        public IReadOnlyCollection<long> Ids { get; }
    }
}