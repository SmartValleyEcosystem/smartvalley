using System;

namespace SmartValley.WebApi.Votings.Responses
{
    public class GetLastSprintResponse
    {
        public bool DoesExist => Sprint != null;

        public SprintResponse Sprint { get; set; }
    }
}
