using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartValley.Domain.Exceptions;

namespace SmartValley.WebApi.ExceptionHandler
{
    public class ModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            var fieldErrors = context
                              .ModelState.Keys
                              .Where(key => context.ModelState[key].Errors.Count > 0)
                              .ToDictionary(
                                  key => key,
                                  key => context.ModelState[key].Errors.Select(x => x.ErrorMessage).ToList());

            throw new AppErrorException(new AppError(ErrorCode.ValidatationError, fieldErrors));
        }
    }
}