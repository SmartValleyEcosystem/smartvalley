using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartValley.WebApi.ScoringApplication.Responses
{
    public class ScoringApplicationBlankResponse
    {
        public DateTimeOffset? Created { get; set; }

        public DateTimeOffset? Saved { get; set; }

        public IEnumerable<ApplicationPartition> Partitions { get; set; }

        public static ScoringApplicationBlankResponse Create(IEnumerable<Domain.Entities.ScoringApplicationQuestion> questions, Domain.ScoringApplication application)
        {
            return new ScoringApplicationBlankResponse
                   {
                       Created = application.Created,
                       Saved = application.Saved,
                       Partitions = questions
                                    .GroupBy(x => new {x.GroupKey, x.GroupOrder})
                                    .OrderBy(x => x.Key.GroupOrder)
                                    .Select(x => new ApplicationPartition
                                                 {
                                                     Name = x.Key.GroupKey,
                                                     Order = x.Key.GroupOrder,
                                                     Questions = x.OrderBy(q => q.Order).Select(q => ScoringApplicationQuestion(application, q))
                                                 })
                                    .ToArray()
                   };
        }

        private static ScoringApplicationQuestion ScoringApplicationQuestion(Domain.ScoringApplication application, Domain.Entities.ScoringApplicationQuestion q)
        {
            return new ScoringApplicationQuestion
                   {
                       Id = q.Id,
                       Key = q.Key,
                       Type = q.Type,
                       ParentId = q.ParentId,
                       ParentTriggerValue = q.ParentTriggerValue,
                       Answer = application.GetAnswer(q.Id),
                       ExtendedInfo = q.ExtendedInfo,
                       Order = q.Order
                   };
        }
    }
}