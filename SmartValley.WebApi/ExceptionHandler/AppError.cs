using System.Collections.Generic;

namespace SmartValley.WebApi.ExceptionHandler
{
    public class AppError
    {
        public string ErrorCode { get; }

        public Dictionary<string, List<string>> Fields { get; }

        public string Message { get; }

        public AppError(ErrorCode errorCode, Dictionary<string, List<string>> fields = null, string message = null)
        {
            ErrorCode = errorCode.ToString();
            Fields = fields ?? new Dictionary<string, List<string>>();
            Message = message;
        }
    }
}