using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartValley.WebApi.WebApi
{
    public abstract class CheckEntityAccessAttribute : ActionFilterAttribute
    {
        private readonly string _parameterName;
        private readonly Type _requestType;

        protected CheckEntityAccessAttribute(string parameterName, Type requestType = null)
        {
            _parameterName = parameterName;
            _requestType = requestType;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (TryExtractId(context, out var id) && !await IsAuthorizedAsync(id, context))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }

        protected abstract Task<bool> IsAuthorizedAsync(long id, ActionExecutingContext context);

        private bool TryExtractId(ActionExecutingContext context, out long id)
        {
            return _requestType != null
                       ? TryExtractIdFromRequest(context, out id)
                       : TryExtractIdFromParameter(context, out id);
        }

        private bool TryExtractIdFromParameter(ActionExecutingContext context, out long id)
        {
            if (context.ActionArguments.TryGetValue(_parameterName, out var valueObject) && valueObject is long value)
            {
                id = value;
                return true;
            }

            id = -1;
            return false;
        }

        private bool TryExtractIdFromRequest(ActionExecutingContext context, out long id)
        {
            var request = context.ActionArguments.Values.FirstOrDefault(a => a.GetType() == _requestType);
            if (request != null && _requestType.GetProperty(_parameterName).GetValue(request) is long value)
            {
                id = value;
                return true;
            }

            id = -1;
            return false;
        }
    }
}