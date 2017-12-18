namespace SmartValley.WebApi.Estimates
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EstimateCommentRequest
    {
        public int QuestionId { get; set; }

        public string Comment { get; set; }
    }
}