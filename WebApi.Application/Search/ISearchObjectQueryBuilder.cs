using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Application.Search
{
    public interface ISearchObjectQueryBuilder
    {
        Task<object> BuildAndExecuteDynamicQueryAsync<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query, CancellationToken cancellationToken = default) where TEntity : Entity;
        Task<object> ExecuteSearchAsync<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query, CancellationToken cancellationToken = default) where TEntity : Entity;
        IQueryable<TEntity> BuildQuery<TEntity>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity;
        IEnumerable<TOut> BuildQuery<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity;
        IQueryable<TEntity> BuildOrderBy<TEntity>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity;
        IEnumerable<TOut> BuildOrderBy<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity;
    }
}
