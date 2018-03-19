using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Experts.Requests
{
    public class AllExpertsRequest
    {
        public AllExpertsRequest()
        {
            Count = 100;
        }

        public int Offset { get; set; }

        [Range(1, 100)]
        public int Count { get; set; }
    }
}