namespace WebApi.Common.DTO.Abstraction
{
    public abstract class IdentifyableDto : IIdentifyable
    {
        public int Id { get; set; }
    }
}
