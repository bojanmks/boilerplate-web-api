using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApi.Application.Exceptions;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Implementation.Core
{
    public class EntityAccessor
    {
        private readonly DbContext _context;

        public EntityAccessor(DbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetQuery<TEntity>(bool includeInactive = false) where TEntity : Entity
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (!includeInactive)
            {
                query = query.Where(x => x.IsActive.Value);
            }

            return query;
        }

        public TEntity Find<TEntity>(int id, bool includeInactive = false) where TEntity : Entity
        {
            return Find<TEntity>(x => x.Id == id, includeInactive);
        }

        public TEntity Find<TEntity>(Expression<Func<TEntity, bool>> expression, bool includeInactive = false) where TEntity : Entity
        {
            return GetQuery<TEntity>(includeInactive).FirstOrDefault(expression);
        }

        public IQueryable<TEntity> FindAll<TEntity>(IEnumerable<int> ids, bool includeInactive = false) where TEntity : Entity
        {
            return FindAll<TEntity>(x => ids.Contains(x.Id), includeInactive);
        }

        public IQueryable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> expression, bool includeInactive = false) where TEntity : Entity
        {
            return GetQuery<TEntity>(includeInactive).Where(expression);
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void AddBatch<TEntity>(IEnumerable<TEntity> entity) where TEntity : class
        {
            _context.Set<TEntity>().AddRange(entity);
        }

        public void Delete<TEntity>(int id, bool forceHardDelete = false) where TEntity : Entity
        {
            var entityToDelete = Find<TEntity>(id);

            if (entityToDelete is null)
            {
                throw new EntityNotFoundException();
            }

            Delete(entityToDelete, forceHardDelete);
        }

        public void DeleteBatch<TEntity>(IEnumerable<int> ids, bool forceHardDelete = false) where TEntity : Entity
        {
            var entitiesToDelete = FindAll<TEntity>(ids);
            DeleteBatch(entitiesToDelete, forceHardDelete);
        }

        public void Delete<TEntity>(TEntity entity, bool forceHardDelete = false) where TEntity : class
        {
            if (entity is ISoftDeletable softDeletableEntity && !forceHardDelete)
            {
                softDeletableEntity.DeletedAt = DateTime.UtcNow;
                softDeletableEntity.IsActive = false;

                return;
            }

            _context.Set<TEntity>().Remove(entity);
        }

        public void DeleteBatch<TEntity>(IEnumerable<TEntity> entities, bool forceHardDelete = false) where TEntity : class
        {
            foreach (var entity in entities)
            {
                Delete(entity, forceHardDelete);
            }
        }

        public bool Exists<TEntity>(int id, bool includeInactive = false) where TEntity : Entity
        {
            return GetQuery<TEntity>(includeInactive).Any(x => x.Id == id);
        }

        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> expression, bool includeInactive = false) where TEntity : Entity
        {
            return GetQuery<TEntity>(includeInactive).Any(expression);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
