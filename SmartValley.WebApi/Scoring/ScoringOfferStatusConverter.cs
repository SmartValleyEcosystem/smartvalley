using System;

namespace SmartValley.WebApi.Scoring
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
                case ScoringOfferStatus.Timeout:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        public static ScoringOfferStatus ToApi(this Domain.Entities.ScoringOfferStatus status, DateTimeOffset expirationTimestamp, DateTimeOffset now)
        {
            switch (status)
            {
                case Domain.Entities.ScoringOfferStatus.Pending:
                    return expirationTimestamp < now ? ScoringOfferStatus.Timeout : ScoringOfferStatus.Pending;
                case Domain.Entities.ScoringOfferStatus.Accepted:
                    return ScoringOfferStatus.Accepted;
                case Domain.Entities.ScoringOfferStatus.Rejected:
                    return ScoringOfferStatus.Rejected;
                case Domain.Entities.ScoringOfferStatus.Finished:
                    return ScoringOfferStatus.Finished;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}