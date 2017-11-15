using System;

namespace SmartValley.WebApi.ExceptionHandler
{
    public class AppErrorException : Exception
    {
        public AppErrorException(ErrorCode code)
        {
            Error = new AppError(code);
        }

        public AppErrorException(ErrorCode code, string message)
        {
            Error = new AppError(code, message: message);
        }

        public AppError Error { get; }
    }
}