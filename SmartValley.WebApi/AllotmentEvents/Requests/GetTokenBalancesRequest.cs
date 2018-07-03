using System.Collections.Generic;

namespace SmartValley.WebApi.AllotmentEvents.Requests
{
    public class GetTokenBalancesRequest
    {
        public IReadOnlyCollection<long> EventsIds { get; set; }
    }
}