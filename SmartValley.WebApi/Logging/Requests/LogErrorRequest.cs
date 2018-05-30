namespace SmartValley.WebApi.Logging.Requests
{
    public class LogErrorRequest : LogRequest
    {
        public string Error { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }

        public override string ToString()
        {
            return $"Error: '{Error}', Message: '{Message}', Url: '{Url}', UserAgent: '{UserAgent}', UserLanguage: '{UserLanguage}'";
        }
    }
}
