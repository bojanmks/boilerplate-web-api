namespace WebApi.Common.DTO.Result
{
    public class FieldErrors
    {
        public string Field { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
