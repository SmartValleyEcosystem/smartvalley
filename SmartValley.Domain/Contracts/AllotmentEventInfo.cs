using System;
using System.Collections.Generic;
using System.Numerics;

namespace SmartValley.Domain.Contracts
{
    public class AllotmentEventInfo
    {
        public AllotmentEventInfo(
            string name,
            AllotmentEventStatus status,
            string tokenContractAddress,
            DateTimeOffset? startDate,
            DateTimeOffset? finishDate,
            int tokenDecimals,
            string tokenTicker,
            BigInteger totalTokensToDistribute,
            IReadOnlyCollection<AllotmentEventParticipantInfo> participants)
        {
            Name = name;
            Status = status;
            TokenContractAddress = tokenContractAddress;
            StartDate = startDate;
            FinishDate = finishDate;
            TokenDecimals = tokenDecimals;
            TokenTicker = tokenTicker;
            Participants = participants;
            TotalTokensToDistribute = totalTokensToDistribute;
        }

        public string Name { get; }

        public AllotmentEventStatus Status { get; }

        public string TokenContractAddress { get; }

        public BigInteger TotalTokensToDistribute { get; set; }

        public DateTimeOffset? StartDate { get; }

        public DateTimeOffset? FinishDate { get; }

        public int TokenDecimals { get; }

        public string TokenTicker { get; }

        public IReadOnlyCollection<AllotmentEventParticipantInfo> Participants { get; }
    }
}