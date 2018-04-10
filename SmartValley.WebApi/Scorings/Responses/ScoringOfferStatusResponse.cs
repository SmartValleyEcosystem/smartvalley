namespace SmartValley.WebApi.Scorings.Responses
{
    public class ScoringOfferStatusResponse
    {
        public bool Exists { get; set; }

        public ScoringOfferStatus? Status { get; set; }
    }
}