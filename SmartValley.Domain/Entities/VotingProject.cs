namespace SmartValley.Domain.Entities
{
    public class VotingProject
    {
        public long ProjectId { get; set; }

        public long VotingId { get; set; }

        public Project Project { get; set; }

        public Voting Voting { get; set; }
    }
}