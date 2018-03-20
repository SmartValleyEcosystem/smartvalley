namespace SmartValley.Domain.Exceptions
{
    public class EmailSendingFailedException : AppErrorException
    {
        public EmailSendingFailedException(string message)
            : base(ErrorCode.EmailSendingFailed, message)
        {
        }
    }
}