using System;

namespace SmartValley.WebApi.Scorings.Requests
{
    public class UpdateOffersRequest
    {
        public Guid ProjectExternalId { get; set; }

        public string TransactionHash { get; set; }
    }
}