using System;

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
            string tokenTicker)
        {
            Name = name;
            Status = status;
            TokenContractAddress = tokenContractAddress;
            StartDate = startDate;
            FinishDate = finishDate;
            TokenDecimals = tokenDecimals;
            TokenTicker = tokenTicker;
        }

        public string Name { get; }

        public AllotmentEventStatus Status { get; }

        public string TokenContractAddress { get; }

        public DateTimeOffset? StartDate { get; }

        public DateTimeOffset? FinishDate { get; }

        public int TokenDecimals { get; }

        public string TokenTicker { get; }
    }
}