namespace WebApi.Application.Logging.LoggerData
{
    public class UseCaseLoggerData
    {
        public int? UserId { get; set; }
        public string UseCaseId { get; set; }
        public bool IsAuthorized { get; set; }
        public DateTime ExecutionDateTime { get; set; }
        public string Data { get; set; }
    }
}
