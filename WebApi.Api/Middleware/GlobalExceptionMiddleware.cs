using Newtonsoft.Json;
using WebApi.Api.ExceptionHandling;
using WebApi.Api.ExceptionHandling.Abstraction;
using WebApi.Application.Logging;

namespace WebApi.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IExceptionResponseGeneratorGetter _responseGeneratorGetter;

        public GlobalExceptionMiddleware(RequestDelegate next, IExceptionLogger exceptionLogger, IExceptionResponseGeneratorGetter responseGeneratorGetter)
        {
            _next = next;
            _exceptionLogger = exceptionLogger;
            _responseGeneratorGetter = responseGeneratorGetter;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExeption(httpContext, ex);
            }
        }

        private Task HandleExeption(HttpContext httpContext, Exception ex)
        {
            var exceptionResponse = new ExceptionResponse();
            var responseGenerator = _responseGeneratorGetter.Get(ex);

            if (responseGenerator is not null)
            {
                exceptionResponse = responseGenerator.Generate(ex);
            }

            if (exceptionResponse.ShouldBeLogged)
            {
                _exceptionLogger.Log(ex);
            }

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = exceptionResponse.StatusCode;

            if (exceptionResponse.Response != null)
            {
                return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(exceptionResponse.Response));
            }

            return Task.FromResult(httpContext.Response);
        }
    }
}
