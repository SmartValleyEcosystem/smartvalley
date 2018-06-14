using System;
using System.Collections.Generic;

namespace SmartValley.Domain
{
    public class AllotmentEventsQuery : CollectionQuery
    {
        public AllotmentEventsQuery(IReadOnlyCollection<AllotmentEventStatus> allotmentEventStatuses, int offset, int count)
            : base(offset, count)
        {
            AllotmentEventStatuses = allotmentEventStatuses ?? throw new ArgumentNullException(nameof(allotmentEventStatuses));
        }

        public IReadOnlyCollection<AllotmentEventStatus> AllotmentEventStatuses { get; }
    }
}