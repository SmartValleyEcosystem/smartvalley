using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class SearchProjectsQuery
    {
        public SearchProjectsQuery(int offset,
                                   int count,
                                   string searchString,
                                   StageType? stageType,
                                   string countryCode,
                                   CategoryType? categoryType,
                                   int? minimumScore,
                                   int? maximumScore,
                                   ProjectsOrderBy? orderBy,
                                   SortDirection? direction)
        {
            Offset = offset;
            Count = count;
            SearchString = searchString;
            StageType = stageType;
            CountryCode = countryCode;
            CategoryType = categoryType;
            MinimumScore = minimumScore;
            MaximumScore = maximumScore;
            OrderBy = orderBy;
            Direction = direction;
        }

        public int Offset { get; }

        public int Count { get; }

        public string SearchString { get; }

        public StageType? StageType { get; }

        public string CountryCode { get; }

        public CategoryType? CategoryType { get; }

        public int? MinimumScore { get; }

        public int? MaximumScore { get; }

        public ProjectsOrderBy? OrderBy { get; }

        public SortDirection? Direction { get; }
    }
}