using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Application.Search
{
    public interface ISearchObjectQueryBuilder
    {
        Task<object> BuildAndExecuteDynamicQueryAsync<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query, CancellationToken cancellationToken = default) where TEntity : Entity;
    }
}
