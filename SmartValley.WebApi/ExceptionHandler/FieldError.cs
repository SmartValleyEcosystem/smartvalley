namespace SmartValley.WebApi.ExceptionHandler
{
    public class FieldError
    {
        public string ErrorCode { get; }

        public FieldError(ValidationErrorCode errorCode)
        {
            ErrorCode = errorCode.ToString();
        }
    }
}