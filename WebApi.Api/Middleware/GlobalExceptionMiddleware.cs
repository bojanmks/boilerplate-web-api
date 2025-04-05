using WebApi.Application.Logging;

namespace WebApi.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionLogger _exceptionLogger;

        public GlobalExceptionMiddleware(RequestDelegate next, IExceptionLogger exceptionLogger)
        {
            _next = next;
            _exceptionLogger = exceptionLogger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }

        private async Task HandleException(HttpContext httpContext, Exception ex)
        {
            await _exceptionLogger.Log(ex);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = 500;
        }
    }
}
