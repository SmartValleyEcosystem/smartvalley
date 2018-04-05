namespace SmartValley.WebApi.Scoring.Responses
{
    public class ScoringOfferStatusResponse
    {
        public bool Exists { get; set; }

        public ScoringOfferStatus? Status { get; set; }
    }
}