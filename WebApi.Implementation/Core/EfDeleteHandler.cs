using WebApi.Application.Core;
using WebApi.DataAccess;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Implementation.Core
{
    public class EfDeleteHandler : IDeleteHandler
    {
        private readonly DatabaseContext _context;

        public EfDeleteHandler(DatabaseContext context)
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
