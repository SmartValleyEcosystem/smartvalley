using System;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class VotingProjectDetails
    {
        public VotingProjectDetails(long projectId, Voting voting, bool isAccepted)
        {
            ProjectId = projectId;
            Voting = voting;
            IsAccepted = isAccepted;
        }

        public long ProjectId { get; }

        public Voting Voting { get; }

        public bool IsAccepted { get; }

        public VotingStatus GetVotingStatus(DateTimeOffset now)
        {
            if (Voting.EndDate > now)
                return VotingStatus.InProgress;

            return IsAccepted ? VotingStatus.Accepted : VotingStatus.Rejected;
        }
    }
}