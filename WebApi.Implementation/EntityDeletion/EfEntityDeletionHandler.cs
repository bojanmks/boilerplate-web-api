using WebApi.Application.EntityDeletion;
using WebApi.DataAccess;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Implementation.EntityDeletion
{
    public class EfEntityDeletionHandler : IEntityDeletionHandler
    {
        private readonly DatabaseContext _context;

        public EfEntityDeletionHandler(DatabaseContext context)
        {
            _context = context;
        }

        public void Delete<TEntity>(TEntity entity, bool forceHardDelete = false) where TEntity : Entity
        {
            if (entity is ISoftDeletable softDeletableEntity && !forceHardDelete)
            {
                softDeletableEntity.DeletedAt = DateTime.UtcNow;
                softDeletableEntity.IsActive = false;

                return;
            }

            _context.Set<TEntity>().Remove(entity);
        }
    }
}
