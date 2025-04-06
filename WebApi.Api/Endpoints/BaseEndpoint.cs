using System.Net;
using FastEndpoints;
using WebApi.Common.DTO.Result;
using WebApi.Common.Enums.Result;

namespace WebApi.Api.Endpoints
{
    public abstract class BaseEndpoint<TRequest, TResponse> : Endpoint<TRequest, EndpointResponse<TResponse>>
        where TRequest : notnull
    {
        protected async Task RespondFromResult(Result<TResponse> result, CancellationToken cancellationToken = default(CancellationToken))
        {
            int statusCode = result.HttpStatusCode.HasValue
                ? result.HttpStatusCode.Value
                : result.Status switch
                {
                    ResultStatus.Success => (int)HttpStatusCode.OK,
                    ResultStatus.Error => (int)HttpStatusCode.InternalServerError,
                    ResultStatus.ValidationError => (int)HttpStatusCode.UnprocessableEntity,
                    ResultStatus.NotFound => (int)HttpStatusCode.NotFound,
                    _ => throw new ArgumentOutOfRangeException(nameof(result.Status), $"Unexpected result status value: {result.Status}")
                };

            var endpointResponse = new EndpointResponse<TResponse>
            {
                Data = result.Data,
                ErrorMessages = result.Errors ?? Enumerable.Empty<string>(),
                FieldErrors = result.FieldErrors ?? Enumerable.Empty<FieldErrors>(),
                StatusCode = statusCode
            };

            await SendAsync(
                endpointResponse,
                statusCode: statusCode,
                cancellation: cancellationToken
            );
        }
    }
}
