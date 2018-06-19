using System;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Scorings
{
    public static class ScoringOfferStatusConverter
    {
        public static ScoringOfferStatus ToApi(this ScoringOfferStatus status, DateTimeOffset acceptingDeadline, DateTimeOffset scoringDeadline, DateTimeOffset now)
        {
            switch (status)
            {
                case ScoringOfferStatus.Pending:
                    return acceptingDeadline < now ? ScoringOfferStatus.Expired : ScoringOfferStatus.Pending;
                case ScoringOfferStatus.Accepted:
                    return scoringDeadline < now ? ScoringOfferStatus.Expired : ScoringOfferStatus.Accepted;
                case ScoringOfferStatus.Rejected:
                    return ScoringOfferStatus.Rejected;
                case ScoringOfferStatus.Finished:
                    return ScoringOfferStatus.Finished;
                case ScoringOfferStatus.Expired:
                    return ScoringOfferStatus.Expired;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}