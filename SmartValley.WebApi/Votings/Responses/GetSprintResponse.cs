namespace SmartValley.WebApi.Votings.Responses
{
    public class GetSprintResponse
    {
        public bool DoesExist => Sprint != null;

        public VotingSprintResponse Sprint { get; set; }
    }
}