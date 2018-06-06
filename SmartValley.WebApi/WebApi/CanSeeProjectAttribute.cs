using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.Projects;

namespace SmartValley.WebApi.WebApi
{
    public class CanSeeProjectAttribute : CheckEntityAccessAttribute
    {
        public CanSeeProjectAttribute(string parameterName, Type requestType = null)
            : base(parameterName, requestType)
        {
        }

        protected override async Task<bool> IsAuthorizedAsync(long id, ActionExecutingContext context)
        {
            var service = context.HttpContext.RequestServices.GetService<IProjectService>();
            var userId = context.HttpContext.User.TryGetUserId();

            return await service.IsAuthorizedToSeeProjectAsync(id, userId);
        }
    }
}