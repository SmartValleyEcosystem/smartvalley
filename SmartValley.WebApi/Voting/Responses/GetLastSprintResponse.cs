using SmartValley.Domain;

namespace SmartValley.WebApi.Voting.Responses
{
    public class GetLastSprintResponse
    {
        public bool DoesExist => LastSprint != null;

        public Sprint LastSprint { get; set; }
    }
}
