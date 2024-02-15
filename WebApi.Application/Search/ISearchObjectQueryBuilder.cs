using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Application.Search
{
    public interface ISearchObjectQueryBuilder
    {
        object BuildDynamicQuery<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity;
        object BuildResponse<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity;
        IQueryable<TEntity> BuildQuery<TEntity>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity;
        IEnumerable<TOut> BuildQuery<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity;
        IQueryable<TEntity> BuildOrderBy<TEntity>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity;
        IEnumerable<TOut> BuildOrderBy<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query) where TEntity : Entity;
    }
}
