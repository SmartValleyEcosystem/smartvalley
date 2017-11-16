using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartValley.Application.Exceptions;

namespace SmartValley.WebApi.ExceptionHandler
{
    public class AppErrorsExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is AppErrorException appErrorsException)
            {
                context.Result = new ObjectResult(appErrorsException.Error)
                                 {
                                     StatusCode = (int) HttpStatusCode.BadRequest
                                 };
            }
            else
            {
                context.Result = new ObjectResult(new AppError(ErrorCode.ServerError, message: context.Exception.ToString()))
                                 {
                                     StatusCode = (int) HttpStatusCode.InternalServerError
                                 };
            }
        }
    }
}