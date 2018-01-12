namespace SmartValley.WebApi.Estimates.Requests
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EstimateCommentRequest
    {
        public int QuestionId { get; set; }

        public string Comment { get; set; }
    }
}