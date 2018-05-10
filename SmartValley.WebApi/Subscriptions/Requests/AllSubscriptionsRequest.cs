using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Subscriptions.Requests
{
    public class AllSubscriptionsRequest
    {
        public AllSubscriptionsRequest()
        {
            Count = 100;
        }

        public int Offset { get; set; }

        [Range(1, 100)]
        public int Count { get; set; }
    }
}