﻿using Newtonsoft.Json;
using WebApi.Application.ExceptionHandling;
using WebApi.Application.Logging;

namespace WebApi.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IExceptionResponseGeneratorResolver _responseGeneratorResolver;

        public GlobalExceptionMiddleware(RequestDelegate next, IExceptionLogger exceptionLogger, IExceptionResponseGeneratorResolver responseGeneratorResolver)
        {
            _next = next;
            _exceptionLogger = exceptionLogger;
            _responseGeneratorResolver = responseGeneratorResolver;
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

        private Task HandleException(HttpContext httpContext, Exception ex)
        {
            var exceptionResponse = new ExceptionResponse();
            var responseGenerator = _responseGeneratorResolver.Resolve(ex);

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
