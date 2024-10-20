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

        public Task<TEntity?> FindByIdAsync<TEntity>(int id, EntityStatusFilter entityStatusFilter = EntityStatusFilter.OnlyActive, CancellationToken cancellationToken = default) where TEntity : Entity
        {
            return FindAsync<TEntity>(x => x.Id == id, entityStatusFilter, cancellationToken);
        }

        public Task<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> expression, EntityStatusFilter entityStatusFilter = EntityStatusFilter.OnlyActive, CancellationToken cancellationToken = default) where TEntity : Entity
        {
            return GetQuery<TEntity>(entityStatusFilter).FirstOrDefaultAsync(expression, cancellationToken);
        }

        public IQueryable<TEntity> FindAllByIds<TEntity>(IEnumerable<int> ids, EntityStatusFilter entityStatusFilter = EntityStatusFilter.OnlyActive) where TEntity : Entity
        {
            return FindAll<TEntity>(x => ids.Contains(x.Id), entityStatusFilter);
        }

        public IQueryable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> expression, EntityStatusFilter entityStatusFilter = EntityStatusFilter.OnlyActive, CancellationToken cancellationToken = default) where TEntity : Entity
        {
            return GetQuery<TEntity>(entityStatusFilter).Where(expression);
        }

        public async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public async Task AddBatchAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
        {
            await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public async Task DeleteByIdAsync<TEntity>(int id, bool forceHardDelete = false, CancellationToken cancellationToken = default) where TEntity : Entity
        {
            var entityToDelete = await FindByIdAsync<TEntity>(id, cancellationToken: cancellationToken);

            if (entityToDelete is null)
            {
                throw new EntityNotFoundException();
            }

            Delete(entityToDelete, forceHardDelete);
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

        public async Task DeleteBatchByIdsAsync<TEntity>(IEnumerable<int> ids, bool forceHardDelete = false, CancellationToken cancellationToken = default) where TEntity : Entity
        {
            var entitiesToDelete = await FindAllByIds<TEntity>(ids).ToListAsync(cancellationToken);
            DeleteBatch(entitiesToDelete, forceHardDelete);
        }

        public void DeleteBatch<TEntity>(IEnumerable<TEntity> entities, bool forceHardDelete = false) where TEntity : class
        {
            foreach (var entity in entities)
            {
                Delete(entity, forceHardDelete);
            }
        }

        public async Task DeactivateByIdAsync<TEntity>(int id, CancellationToken cancellationToken = default) where TEntity : Entity
        {
            var entityToDeactivate = await FindByIdAsync<TEntity>(id, cancellationToken: cancellationToken);

            if (entityToDeactivate is null)
            {
                throw new EntityNotFoundException();
            }

            Deactivate(entityToDeactivate);
        }

        public void Deactivate<TEntity>(TEntity entity) where TEntity : Entity
        {
            entity.IsActive = false;
        }

        public async Task DeactivateBatchByIdsAsync<TEntity>(IEnumerable<int> ids, CancellationToken cancellationToken = default) where TEntity : Entity
        {
            var entitiesToDeactivate = await FindAllByIds<TEntity>(ids).ToListAsync(cancellationToken);
            DeactivateBatch(entitiesToDeactivate);
        }

        public void DeactivateBatch<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        {
            foreach (var entity in entities)
            {
                Deactivate(entity);
            }
        }

        public Task<bool> ExistsByIdAsync<TEntity>(int id, EntityStatusFilter entityStatusFilter = EntityStatusFilter.OnlyActive, CancellationToken cancellationToken = default) where TEntity : Entity
        {
            return GetQuery<TEntity>(entityStatusFilter).AnyAsync(x => x.Id == id, cancellationToken);
        }

        public Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> expression, EntityStatusFilter entityStatusFilter = EntityStatusFilter.OnlyActive, CancellationToken cancellationToken = default) where TEntity : Entity
        {
            return GetQuery<TEntity>(entityStatusFilter).AnyAsync(expression, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
