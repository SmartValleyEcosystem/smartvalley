using System;
using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.ScoringApplications.Responses
{
    public class ScoringApplicationResponse
    {
        public DateTimeOffset? Created { get; set; }

        public DateTimeOffset? Saved { get; set; }

        public ProjectApplicationInfoResponse ProjectInfo { get; set; }

        public IEnumerable<ApplicationPartitionResponse> Partitions { get; set; }

        public bool IsSubmitted { get; set; }

        public static ScoringApplicationResponse CreateEmpty(IEnumerable<ScoringApplicationQuestion> questions, Project project, Country projectCountry, IReadOnlyCollection<ProjectTeamMember> projectTeamMembers)
        {
            var partitions = CreatePartitions(questions);
            var projectInfo = ProjectApplicationInfoResponse.CreateFrom(project, projectCountry, projectTeamMembers);

            return new ScoringApplicationResponse
                   {
                       ProjectInfo = projectInfo,
                       Partitions = partitions,
                       IsSubmitted = false
                   };
        }

        public static ScoringApplicationResponse InitializeFromApplication(IEnumerable<ScoringApplicationQuestion> questions, ScoringApplication application)
        {
            var partitions = CreatePartitions(questions);
            var projectInfo = ProjectApplicationInfoResponse.CreateFrom(application);
            var blank = new ScoringApplicationResponse
                        {
                            ProjectInfo = projectInfo,
                            Partitions = partitions,
                            Created = application.Created,
                            Saved = application.Saved
                        };

            blank.SetAnswersFromApplication(application);
            blank.IsSubmitted = application.IsSubmitted;
            return blank;
        }

        private static IEnumerable<ApplicationPartitionResponse> CreatePartitions(IEnumerable<ScoringApplicationQuestion> questions)
        {
            return questions
                   .GroupBy(x => new {x.GroupKey, x.GroupOrder})
                   .OrderBy(x => x.Key.GroupOrder)
                   .Select(x => new ApplicationPartitionResponse
                                {
                                    Name = x.Key.GroupKey,
                                    Order = x.Key.GroupOrder,
                                    Questions = x.OrderBy(q => q.Order).Select(CreateEmptyQuestion).ToArray()
                                })
                   .ToArray();
        }

        private static ScoringApplicationQuestionResponse CreateEmptyQuestion(Domain.Entities.ScoringApplicationQuestion q)
        {
            return new ScoringApplicationQuestionResponse
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