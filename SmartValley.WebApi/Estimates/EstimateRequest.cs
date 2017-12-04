namespace SmartValley.WebApi.Estimates
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EstimateRequest
    {
        public int QuestionId { get; set; }

        public int Score { get; set; }

        public string Comment { get; set; }
    }
}