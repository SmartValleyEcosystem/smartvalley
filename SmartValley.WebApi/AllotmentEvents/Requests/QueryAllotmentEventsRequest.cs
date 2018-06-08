using System.Collections.Generic;
using SmartValley.Domain;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.AllotmentEvents.Requests
{
    public class QueryAllotmentEventsRequest : CollectionPageRequest
    {
        public IReadOnlyCollection<AllotmentEventStatus> AllotmentEventStatuses { get; set; }
    }
}