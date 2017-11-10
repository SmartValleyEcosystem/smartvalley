using System;

namespace SmartValley.WebApi.ExceptionHandler
{
    public class AppErrorException : Exception
    {
        public AppError Error { get; }

        public AppErrorException(AppError error)
        {
            Error = error;
        }
    }
}