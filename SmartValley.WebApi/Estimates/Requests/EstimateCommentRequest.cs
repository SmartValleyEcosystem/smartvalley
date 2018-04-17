﻿using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates.Requests
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EstimateCommentRequest
    {
        public int ScoringCriterionId { get; set; }

        public string Comment { get; set; }

        public Score Score { get; set; }
    }
}