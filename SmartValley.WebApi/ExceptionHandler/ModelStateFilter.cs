using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartValley.WebApi.ExceptionHandler
{
    public class ModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;
            var fieldErrors = context.ModelState.Keys
                                     .ToDictionary(key => key, key => context.ModelState[key].Errors
                                                                             .Select(x => x.ErrorMessage).ToList());

            throw new AppErrorException(new AppError(ErrorCode.ValidatationError, fieldErrors));
        }
    }
}
