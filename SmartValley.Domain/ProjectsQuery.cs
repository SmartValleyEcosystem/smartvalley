using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ProjectsQuery
    {
        public ProjectsQuery(int offset,
                             int count,
                             bool onlyScored,
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
            OnlyScored = onlyScored;
        }

        public int Offset { get; }

        public int Count { get; }

        public bool OnlyScored { get; }

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