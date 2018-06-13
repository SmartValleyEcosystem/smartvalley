namespace SmartValley.WebApi.AllotmentEvents.Responses
{
    public class CreateAllotmentEventResponse
    {
        public CreateAllotmentEventResponse(long allotmentEventId)
        {
            AllotmentEventId = allotmentEventId;
        }

        public long AllotmentEventId { get; }
    }
}