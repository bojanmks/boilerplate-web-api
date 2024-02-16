namespace WebApi.DataAccess.Entities.Abstraction
{
    public class SoftDeletableEntity : Entity, ISoftDeletable
    {
        public DateTime? DeletedAt { get; set; }
    }
}
