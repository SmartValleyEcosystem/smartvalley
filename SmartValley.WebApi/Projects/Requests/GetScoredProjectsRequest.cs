using System.ComponentModel.DataAnnotations;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Requests
{
    public class GetScoredProjectsRequest
    {
        public GetScoredProjectsRequest()
        {
            Count = 100;
        }

        public int Offset { get; set; }

        [Range(1, 100)]
        public int Count { get; set; }

        public string SearchString { get; set; }

        public StageType? StageType { get; set; }

        public string CountryCode { get; set; }

        public CategoryType? CategoryType { get; set; }

        public int? MinimumScore { get; set; }

        public int? MaximumScore { get; set; }

        public ProjectsOrderBy? OrderBy { get; set; }

        public SortDirection? Direction { get; set; }
    }
}