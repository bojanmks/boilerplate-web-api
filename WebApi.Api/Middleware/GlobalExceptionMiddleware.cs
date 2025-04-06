using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApi.Api.Endpoints;
using WebApi.Application.Localization;
using WebApi.Application.Logging;
using WebApi.Application.UseCases;

namespace WebApi.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly ITranslator _translator;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            IExceptionLogger exceptionLogger,
            ITranslator translator)
        {
            _next = next;
            _exceptionLogger = exceptionLogger;
            _translator = translator;
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

            int statusCode = (int)HttpStatusCode.InternalServerError;

            var responseBody = new EndpointResponse<Empty>
            {
                Data = Empty.Value,
                ErrorMessages = new string[] { _translator.Translate("anErrorOccurred") },
                StatusCode = statusCode
            };

            var serializedBody = JsonConvert.SerializeObject(
                responseBody,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            );

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsync(serializedBody);
        }
    }
}
