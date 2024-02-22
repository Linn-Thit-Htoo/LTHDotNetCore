namespace LTHDotNetCore.RestApi.Middlewares
{
    public class LogHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogHeadersMiddleware> _logger;

        public LogHeadersMiddleware(RequestDelegate next, ILogger<LogHeadersMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            foreach (var header in context.Request.Headers)
            {
                _logger.LogInformation("Header: {Key}: {Value}", header.Key, header.Value);
            }

            await _next(context);
        }
    }

    public static class LogHeadersMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogHeaders(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogHeadersMiddleware>();
        }
    }
}
