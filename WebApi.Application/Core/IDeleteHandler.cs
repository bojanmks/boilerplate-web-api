using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Application.Core
{
    public interface IDeleteHandler
    {
        void Delete<TEntity>(TEntity entity, bool forceHardDelete = false) where TEntity : Entity;
    }
}
