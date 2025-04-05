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
                    ResultStatus.Success => 200,
                    ResultStatus.Error => 500,
                    ResultStatus.ValidationError => 422,
                    _ => throw new ArgumentOutOfRangeException(nameof(result.Status), $"Unexpected result status value: {result.Status}")
                };

            var endpointResponse = new EndpointResponse<TResponse>
            {
                Data = result.Data,
                ErrorMessages = result.Errors,
                StatusCode = statusCode
            };

            await SendAsync(endpointResponse, cancellation: cancellationToken);
        }
    }
}
