using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Feedbacks.Requests
{
    public class AllFeedbacksRequest
    {
        public AllFeedbacksRequest()
        {
            Count = 100;
        }

        public int Offset { get; set; }

        [Range(1, 100)]
        public int Count { get; set; }
    }
}