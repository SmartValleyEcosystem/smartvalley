using System;
using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.AllotmentEvents.Requests
{
    public class CreateAllotmentEventRequest
    {
        public long ProjectId { get; set; }
        
        [Required, MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public string TokenContractAddress { get; set; }

        [Required, MaxLength(6)]
        public string TokenTicker { get; set; }
        
        [Required, Range(0, 18)]
        public int TokenDecimals { get; set; }

        public DateTimeOffset? FinishDate { get; set; }
    }
}