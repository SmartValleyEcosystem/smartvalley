namespace SmartValley.WebApi.ScoringApplication.Requests
{
    public class ScoringApplicationAnswerRequest
    {
        public int QuestionId { get; set; }

        public string Value { get; set; }

        public static Domain.ScoringApplicationAnswer ToDomain(ScoringApplicationAnswerRequest answerRequest)
        {
            return new Domain.ScoringApplicationAnswer
                   {
                       Value = answerRequest.Value,
                       QuestionId = answerRequest.QuestionId
                   };
        }
    }
}