using System;

namespace SmartValley.WebApi.ExceptionHandler
{
    public class AppErrorException : Exception
    {
        public AppErrorException(ErrorCode code)
        {
            Error = new AppError(code);
        }

        public AppError Error { get; }
    }
}