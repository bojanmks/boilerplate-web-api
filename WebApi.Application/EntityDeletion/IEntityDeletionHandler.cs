using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Application.EntityDeletion
{
    public interface IEntityDeletionHandler
    {
        void Delete<TEntity>(TEntity entity, bool forceHardDelete = false) where TEntity : Entity;
    }
}
