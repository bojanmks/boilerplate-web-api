using WebApi.Common.DTO.Result;

namespace WebApi.Api.Endpoints
{
    public class EndpointResponse<T>
    {
        public T Data { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<FieldErrors> FieldErrors { get; set; } = Enumerable.Empty<FieldErrors>();
        public int StatusCode { get; set; }
    }
}
