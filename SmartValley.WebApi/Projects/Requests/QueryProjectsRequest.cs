﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Requests
{
    public class QueryProjectsRequest
    {
        public QueryProjectsRequest()
        {
            Count = 100;
            ScoringStatuses = new List<ScoringStatus>();
        }

        public int Offset { get; set; }

        [Range(1, 100)]
        public int Count { get; set; }

        public bool OnlyScored { get; set; }

        public bool? IsPrivate { get; set; }

        public string SearchString { get; set; }

        public Stage? Stage { get; set; }

        public string CountryCode { get; set; }

        public Category? Category { get; set; }

        public int? MinimumScore { get; set; }

        public int? MaximumScore { get; set; }

        public ProjectsOrderBy? OrderBy { get; set; }

        public SortDirection? SortDirection { get; set; }

        public IReadOnlyCollection<ScoringStatus> ScoringStatuses { get; set; }

        public IReadOnlyCollection<long> ProjectIds { get; set; }
    }
}