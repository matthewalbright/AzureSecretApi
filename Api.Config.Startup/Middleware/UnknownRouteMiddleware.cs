using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Runtime;
using System.Threading.Tasks;

namespace Api.Config.Startup.Middleware
{
    public class UnknownRouteMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public UnknownRouteMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<UnknownRouteMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var route = context.Request.Path.Value;

            if (route == "/")
            {

                const string swaggerUri = "/swagger/index.html";

                var uriScheme = (context.Request.IsHttps ? "https://" : "http://");

                context.Response.Redirect($"{uriScheme}{context.Request.Host}{swaggerUri}", true);
            }
            else
            {
                var msg = $"{(int)HttpStatusCode.NotFound}: Route Not Found. Route={context.Request.Path}";

                _logger.LogInformation(msg);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsync(msg);
            }
        }
    }

    public static class UnknownRouteMiddlewareExtension
    {
        public static IApplicationBuilder UseUnknownRoute(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UnknownRouteMiddleware>();
        }
    }
}
