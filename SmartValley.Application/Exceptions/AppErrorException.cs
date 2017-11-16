using System;

namespace SmartValley.Application.Exceptions
{
    public class AppErrorException : Exception
    {
        public AppErrorException(ErrorCode code)
        {
            Error = new AppError(code);
        }
        public AppErrorException(AppError error)
        {
            Error = error;
        }

        public AppErrorException(ErrorCode code, string message)
        {
            Error = new AppError(code, message: message);
        }

        public AppError Error { get; }
    }
}