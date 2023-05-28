﻿using FitLog.Api.ExceptionHandling.Abstraction;

namespace FitLog.Api.ExceptionHandling.ResponseGenerators
{
    public class UnauthorizedAccessExceptionResponseGenerator : ExceptionResponseGenerator<UnauthorizedAccessException>
    {
        protected override ExceptionResponse GenerateAfterCast(UnauthorizedAccessException ex)
        {
            return new ExceptionResponse
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                ShouldBeLogged = false
            };
        }
    }
}
