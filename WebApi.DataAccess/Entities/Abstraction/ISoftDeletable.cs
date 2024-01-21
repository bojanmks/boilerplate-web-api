namespace WebApi.DataAccess.Entities.Abstraction
{
    public interface ISoftDeletable
    {
        public DateTime? DeletedAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
