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

        public static ScoringApplicationBlankResponse CreateEmpty(IEnumerable<Domain.Entities.ScoringApplicationQuestion> questions)
        {
            return new ScoringApplicationBlankResponse
                   {
                       Partitions = questions
                                    .GroupBy(x => new { x.GroupKey, x.GroupOrder })
                                    .OrderBy(x => x.Key.GroupOrder)
                                    .Select(x => new ApplicationPartition
                                                 {
                                                     Name = x.Key.GroupKey,
                                                     Order = x.Key.GroupOrder,
                                                     Questions = x.OrderBy(q => q.Order).Select(GetEmptyScoringApplicationQuestion).ToArray()
                                                 })
                                    .ToArray()
                   };
        }

        public static ScoringApplicationBlankResponse InitializeFromApplication(IEnumerable<Domain.Entities.ScoringApplicationQuestion> questions, Domain.ScoringApplication application)
        {
            var blank = CreateEmpty(questions);

            blank.Created = application.Created;
            blank.Saved = application.Saved;

            foreach (var answer in application.Answers)
            {
                var question = blank.Partitions.SelectMany(x => x.Questions).FirstOrDefault(x => x.Id == answer.QuestionId);
                if (question != null)
                {
                    question.Answer = answer.Value;
                }
            }

            return blank;
        }
        
        private static ScoringApplicationQuestion GetEmptyScoringApplicationQuestion(Domain.Entities.ScoringApplicationQuestion q)
        {
            return new ScoringApplicationQuestion
                   {
                       Id = q.Id,
                       Key = q.Key,
                       Type = q.Type,
                       ParentId = q.ParentId,
                       ParentTriggerValue = q.ParentTriggerValue,
                       ExtendedInfo = q.ExtendedInfo,
                       Order = q.Order
                   };
        }
    }
}