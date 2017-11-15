using System;

namespace SmartValley.WebApi.ExceptionHandler
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

        public AppError Error { get; }
    }
}