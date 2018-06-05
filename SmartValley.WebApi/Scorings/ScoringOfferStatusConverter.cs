using System;

namespace SmartValley.WebApi.Scorings
{
    public static class ScoringOfferStatusConverter
    {
        public static Domain.Entities.ScoringOfferStatus? ToDomain(this ScoringOfferStatus status)
        {
            switch (status)
            {
                case ScoringOfferStatus.Pending:
                    return Domain.Entities.ScoringOfferStatus.Pending;
                case ScoringOfferStatus.Accepted:
                    return Domain.Entities.ScoringOfferStatus.Accepted;
                case ScoringOfferStatus.Rejected:
                    return Domain.Entities.ScoringOfferStatus.Rejected;
                case ScoringOfferStatus.Finished:
                    return Domain.Entities.ScoringOfferStatus.Finished;
                case ScoringOfferStatus.Expired:
                    return Domain.Entities.ScoringOfferStatus.Expired;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        public static ScoringOfferStatus ToApi(this Domain.Entities.ScoringOfferStatus status, DateTimeOffset acceptingDeadline, DateTimeOffset scoringDeadline, DateTimeOffset now)
        {
            switch (status)
            {
                case Domain.Entities.ScoringOfferStatus.Pending:
                    return acceptingDeadline < now ? ScoringOfferStatus.Expired : ScoringOfferStatus.Pending;
                case Domain.Entities.ScoringOfferStatus.Accepted:
                    return scoringDeadline < now ? ScoringOfferStatus.Expired : ScoringOfferStatus.Accepted;
                case Domain.Entities.ScoringOfferStatus.Rejected:
                    return ScoringOfferStatus.Rejected;
                case Domain.Entities.ScoringOfferStatus.Finished:
                    return ScoringOfferStatus.Finished;
                case Domain.Entities.ScoringOfferStatus.Expired:
                    return ScoringOfferStatus.Expired;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}