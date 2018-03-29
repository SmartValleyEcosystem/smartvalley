using System;
using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringApplication
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Saved { get; set; }

        public DateTimeOffset? Submitted { get; set; }

        public string ProjectName { get; set; }

        public string Category { get; set; }

        public string Status { get; set; }

        public string ProjectDescription { get; set; }

        public string Site { get; set; }

        public string WhitePaper { get; set; }

        public string IcoDate { get; set; }

        public string ContactEmail { get; set; }

        public SocialNetworks SocialNetworks { get; set; }

        public long? CountryId { get; set; }

        public Country Country { get; set; }

        public long ProjectId { get; set; }

        public Project Project { get; set; }

        public ICollection<ScoringApplicationAnswer> Answers { get; set; }

        public ICollection<ScoringApplicationTeamMember> TeamMembers { get; set; }

        public ICollection<ScoringApplicationAdviser> Advisers { get; set; }

        public static ScoringApplication Create(DateTimeOffset currentDate)
        {
            return new ScoringApplication
                   {
                       Answers = new List<ScoringApplicationAnswer>(),
                       TeamMembers = new List<ScoringApplicationTeamMember>(),
                       Advisers = new List<ScoringApplicationAdviser>(),
                       Created = currentDate
                   };
        }

        public void UpdateTeamMembers(IReadOnlyCollection<ScoringApplicationTeamMember> teamMembers)
        {
            TeamMembers.Clear();
            foreach (var teamMember in teamMembers)
            {
                TeamMembers.Add(teamMember);
            }
        }

        public void UpdateAdvisers(IReadOnlyCollection<ScoringApplicationAdviser> advisers)
        {
            Advisers.Clear();
            foreach (var adviser in advisers)
            {
                Advisers.Add(adviser);
            }
        }

        public void UpdateAnswers(IDictionary<int, string> answers)
        {
            Answers.Clear();
            foreach (var answer in answers)
            {
                SetAnswer(answer.Key, answer.Value);
            }
        }

        public string GetAnswer(long questionId)
        {
            return Answers.FirstOrDefault(x => x.QuestionId == questionId)?.Value;
        }

        public void SetAnswer(long questionid, string value)
        {
            var answer = Answers.FirstOrDefault(x => x.QuestionId == questionid);
            if (answer == null)
            {
                answer = new ScoringApplicationAnswer
                         {
                             QuestionId = questionid
                         };
                Answers.Add(answer);
            }

            answer.Value = value;
        }
    }
}