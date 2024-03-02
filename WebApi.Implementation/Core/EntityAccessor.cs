using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApi.Application.Exceptions;
using WebApi.Common.Enums;
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

        public IQueryable<TEntity> GetQuery<TEntity>(EntityStatusFilter entityStatusFilter = EntityStatusFilter.OnlyActive) where TEntity : Entity
        {
            var query = _context.Set<TEntity>().AsQueryable();

            switch (entityStatusFilter)
            {
                case EntityStatusFilter.OnlyActive:
                    query = query.Where(x => x.IsActive.Value);
                    break;

                case EntityStatusFilter.IncludeInactive:
                    if (typeof(TEntity).IsAssignableFrom(typeof(ISoftDeletable)))
                    {
                        query = query.Where(x => ((ISoftDeletable)x).DeletedAt == null);
                    }
                    break;
            }

            return query;
        }

        public TEntity Find<TEntity>(int id, EntityStatusFilter entityStatusFilter = EntityStatusFilter.OnlyActive) where TEntity : Entity
        {
            return Find<TEntity>(x => x.Id == id, entityStatusFilter);
        }

        public TEntity Find<TEntity>(Expression<Func<TEntity, bool>> expression, EntityStatusFilter entityStatusFilter = EntityStatusFilter.OnlyActive) where TEntity : Entity
        {
            return GetQuery<TEntity>(entityStatusFilter).FirstOrDefault(expression);
        }

        public IQueryable<TEntity> FindAll<TEntity>(IEnumerable<int> ids, EntityStatusFilter entityStatusFilter = EntityStatusFilter.OnlyActive) where TEntity : Entity
        {
            return FindAll<TEntity>(x => ids.Contains(x.Id), entityStatusFilter);
        }

        public IQueryable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> expression, EntityStatusFilter entityStatusFilter = EntityStatusFilter.OnlyActive) where TEntity : Entity
        {
            return GetQuery<TEntity>(entityStatusFilter).Where(expression);
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

        public void Deactivate<TEntity>(int id) where TEntity : Entity
        {
            var entityToDeactivate = Find<TEntity>(id);

            if (entityToDeactivate is null)
            {
                throw new EntityNotFoundException();
            }

            Deactivate(entityToDeactivate);
        }

        public void DeactivateBatch<TEntity>(IEnumerable<int> ids) where TEntity : Entity
        {
            var entitiesToDeactivate = FindAll<TEntity>(ids);
            DeactivateBatch(entitiesToDeactivate);
        }

        public void Deactivate<TEntity>(TEntity entity) where TEntity : Entity
        {
            entity.IsActive = false;
        }

        public void DeactivateBatch<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        {
            foreach (var entity in entities)
            {
                Deactivate(entity);
            }
        }

        public bool Exists<TEntity>(int id, EntityStatusFilter entityStatusFilter = EntityStatusFilter.OnlyActive) where TEntity : Entity
        {
            return GetQuery<TEntity>(entityStatusFilter).Any(x => x.Id == id);
        }

        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> expression, EntityStatusFilter entityStatusFilter = EntityStatusFilter.OnlyActive) where TEntity : Entity
        {
            return GetQuery<TEntity>(entityStatusFilter).Any(expression);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
