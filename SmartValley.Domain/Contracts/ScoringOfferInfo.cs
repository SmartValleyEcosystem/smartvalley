using System;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Contracts
{
    public class ScoringOfferInfo
    {
        public ScoringOfferInfo(
            string expertAddress,
            AreaType area,
            ScoringOfferStatus status)
        {
            ExpertAddress = expertAddress;
            Area = area;
            Status = status;
        }

        public string ExpertAddress { get; }

        public AreaType Area { get; }

        public ScoringOfferStatus Status { get; }
    }
}