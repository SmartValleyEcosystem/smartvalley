using SmartValley.Domain;

namespace SmartValley.WebApi.Votings.Responses
{
    public class GetLastSprintResponse
    {
        public bool DoesExist => LastSprint != null;

        public VotingSprintResponse LastSprint { get; set; }
    }
}