using System;
using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.ScoringApplication.Responses
{
    public class ScoringApplicationBlankResponse
    {
        public DateTimeOffset? Created { get; set; }

        public DateTimeOffset? Saved { get; set; }

        public ProjectApplicationInfoResponse ProjectInfo { get; set; }

        public IEnumerable<ApplicationPartition> Partitions { get; set; }

        public static ScoringApplicationBlankResponse CreateEmpty(IEnumerable<Domain.Entities.ScoringApplicationQuestion> questions, Project project, Country projectCountry, IReadOnlyCollection<ProjectTeamMember> projectTeamMembers)
        {
            var partitions = CreatePartitions(questions);
            var projectInfo = ProjectApplicationInfoResponse.CreateFrom(project, projectCountry, projectTeamMembers);

            return new ScoringApplicationBlankResponse
                   {
                       ProjectInfo = projectInfo,
                       Partitions = partitions
                   };
        }

        public static ScoringApplicationBlankResponse InitializeFromApplication(IEnumerable<Domain.Entities.ScoringApplicationQuestion> questions, Domain.ScoringApplication application)
        {
            var partitions = CreatePartitions(questions);
            var projectInfo = ProjectApplicationInfoResponse.CreateFrom(application);
            var blank = new ScoringApplicationBlankResponse
                        {
                            ProjectInfo = projectInfo,
                            Partitions = partitions,
                            Created = application.Created,
                            Saved = application.Saved
                        };

            blank.SetAnswersFromApplication(application);
            return blank;
        }

        private static IEnumerable<ApplicationPartition> CreatePartitions(IEnumerable<Domain.Entities.ScoringApplicationQuestion> questions)
        {
            return questions
                   .GroupBy(x => new { x.GroupKey, x.GroupOrder })
                   .OrderBy(x => x.Key.GroupOrder)
                   .Select(x => new ApplicationPartition
                                {
                                    Name = x.Key.GroupKey,
                                    Order = x.Key.GroupOrder,
                                    Questions = x.OrderBy(q => q.Order).Select(CreateEmptyQuestion).ToArray()
                                })
                   .ToArray();
        }

        private static ScoringApplicationQuestion CreateEmptyQuestion(Domain.Entities.ScoringApplicationQuestion q)
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

        private void SetAnswersFromApplication(Domain.ScoringApplication application)
        {
            foreach (var answer in application.Answers)
            {
                var question = Partitions.SelectMany(x => x.Questions).FirstOrDefault(x => x.Id == answer.QuestionId);
                if (question != null)
                {
                    question.Answer = answer.Value;
                }
            }
        }
    }
}