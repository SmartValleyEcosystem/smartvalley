namespace SmartValley.WebApi.Experts.Requests
{
    public class ExpertUpdateRequest
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string About { get; set; }

        public bool IsAvailable { get; set; }
    }
}