using SmartValley.WebApi.Experts;

namespace SmartValley.WebApi.Scorings.Requests
{
    public class GetScoringOfferStatusRequest
    {
        public long ProjectId { get; set; }

        public AreaType AreaType { get; set; }
    }
}