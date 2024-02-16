using WebApi.Common.DTO.Abstraction;

namespace WebApi.DataAccess.Entities.Abstraction
{
    public abstract class Entity : IIdentifyable
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
