namespace SmartValley.WebApi.Votings.Responses
{
    public class GetCurrentSprintResponse
    {
        public bool DoesExist => CurrentSprint != null;

        public VotingSprintResponse CurrentSprint { get; set; }
    }
}