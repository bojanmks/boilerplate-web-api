namespace WebApi.Api.Endpoints
{
    public class EndpointResponse<T>
    {
        public T Data { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
        public int StatusCode { get; set; }
    }
}
