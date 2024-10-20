namespace WebApi.Common.DTO
{
    public abstract class IdentifyableDto : IIdentifyable
    {
        public int Id { get; set; }
    }
}
