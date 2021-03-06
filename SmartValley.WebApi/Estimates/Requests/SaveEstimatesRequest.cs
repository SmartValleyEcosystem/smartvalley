﻿using System.Collections.Generic;
using SmartValley.WebApi.Experts;

namespace SmartValley.WebApi.Estimates.Requests
{
    public class SaveEstimatesRequest
    {
        public long ProjectId { get; set; }

        public AreaType AreaType { get; set; }

        public IReadOnlyCollection<EstimateRequest> EstimateComments { get; set; }

        public string Conclusion { get; set; }
    }
}