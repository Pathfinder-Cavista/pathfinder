using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using PathFinder.Application.Settings;

namespace PathFinder.API.Filters
{
    public class AnalyticsPermissionFilter : IAsyncActionFilter
    {
        private readonly IOptions<AnalyticsSettings> options;

        public AnalyticsPermissionFilter(IOptions<AnalyticsSettings> options)
        {
            this.options = options ?? 
                throw new ArgumentNullException(nameof(options));
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var headers = context.HttpContext.Request.Headers;
            if(!headers.TryGetValue(options.Value.Header, out var value) || value != options.Value.Value)
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Content = "Permission denied"
                };

                return;
            }

            await next();
        }
    }
}
