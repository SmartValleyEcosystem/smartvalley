using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Experts.Requests
{
    public class QueryExpertsRequest : CollectionPageRequest
    {
        public bool? IsInHouse { get; set; }
    }
}
