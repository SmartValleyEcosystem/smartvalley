using System;
using System.Collections.Generic;

namespace SmartValley.Domain.Contracts
{
    public class ScoringInfo
    {
        public ScoringInfo(Guid projectExternalId,
            IReadOnlyCollection<ScoringOfferInfo> offers,
                           DateTimeOffset acceptingDeadline,
                           DateTimeOffset scoringDeadline)
        {
            ProjectExternalId = projectExternalId;
            Offers = offers;
            AcceptingDeadline = acceptingDeadline;
            ScoringDeadline = scoringDeadline;
        }

        public Guid ProjectExternalId { get; }

        public IReadOnlyCollection<ScoringOfferInfo> Offers { get; }

        public DateTimeOffset AcceptingDeadline { get; }

        public DateTimeOffset ScoringDeadline { get; }
    }
}