using WebApi.Common.DTO.Result;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Application.Search
{
    public interface ISearchObjectQueryBuilder
    {
        Task<Result<object>> BuildAndExecuteDynamicQueryAsync<TEntity, TOut>(ISearchObject search, IQueryable<TEntity> query, CancellationToken cancellationToken = default) where TEntity : Entity;
    }
}
